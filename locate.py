from flask import Flask, Response, send_from_directory, jsonify
from flask_restful import Resource, Api
import googlemaps, datetime, re, json, requests, os

app = Flask(__name__)
api = Api(app)

GOOGLE_MAPS_KEY = "AIzaSyAAkSqy8J5A3scqAfvn3YKLEGvNFhDEPKc"
gmaps = googlemaps.Client(GOOGLE_MAPS_KEY)

pattern = re.compile('([^<]*)<[^>]*>')

def calculate_distance_time(lat1, lng1, lat2, lng2):
	res = gmaps.directions(str(lat1)+","+str(lng1),
		str(lat2)+","+str(lng2),
		mode="walking",
		departure_time=datetime.datetime.now())
	
	res = res[0]["legs"][0]
	
	distance = res["distance"]["value"]
	duration = res["duration"]["value"]
	
	return distance, duration

def feed_steps(lat1, lng1, lat2, lng2):
	res = gmaps.directions(str(lat1)+","+str(lng1),
		str(lat2)+","+str(lng2),
		mode="walking",
		departure_time=datetime.datetime.now())

	res = res[0]["legs"][0]

	steps_list = list()
	end_coords = list()
	for x in res["steps"]:
		if("html_instructions" in x.keys()):
			x["html_instructions"] = x["html_instructions"].replace('<b>','')
			x["html_instructions"] = x["html_instructions"].replace('</b>','')
			x["html_instructions"] = x["html_instructions"].replace('&nbsp;','')
			step = re.findall(pattern, x["html_instructions"])
			try:
				for i in range(len(step)):
					try:
						if(len(step[i]) == 0):
							step.remove(step[i])
					except:
						pass
			except:
				pass

			if(len(step)==0):
				steps_list.append(x["html_instructions"])
			else:
				steps_list.append(step)

			coords = [ x["end_location"]["lat"], x["end_location"]["lng"] ]
			end_coords.append(coords)

	o = open('abc.txt','w')

	for i in range(len(steps_list)):
		step = dict()
		step.update({'step' : [{'direction' : steps_list[i]}, {'location' : end_coords[i]}]})
		o.write(json.dumps(step)+'\n')

	o.close()

	original_distance = calculate_distance_time(lat1, lng1, lat2, lng2)[0]
	o = open("distance.txt","w")
	o.write(str(original_distance))
	o.close()

def get_current_status(current_lat, current_lng):
	o = open("abc.txt",'r')
	steps_list = o.read()
	o.close()
	steps_list = steps_list.split('\n')
	steps_list = steps_list[:len(steps_list)-1]
	step = steps_list[0]
	step = json.loads(step)
	lat,lng = step["step"][1]["location"]
	distance = calculate_distance_time(lat, lng, current_lat, current_lng)[0]

	o = open("distance.txt","r")
	original_distance = o.read()
	o.close()
	original_distance = float(original_distance)

	message = None
	result = dict()
	
	if(len(steps_list) == 1):
		if(distance <= 5):
			message = "You've reached your destination"
			result.update({'message' : message})
		else:
			message = step["step"][0]["direction"]
			result.update({'message' : message})

	elif(distance > original_distance):
		message = "alert"
		result.update({'message' : message})

	elif(distance <= 5):
		message = step["step"][0]["direction"]
		result.update({'message' : message})
		steps_list = steps_list[1:]
		
		o = open('abc.txt','w')
		
		for i in range(len(steps_list)):
			step = dict()
			step.update({'step' : [{'direction' : steps_list[i]}, {'location' : end_coords[i]}]})
			#steps.put(step)
			o.write(json.dumps(step)+'\n')

		o.close()

	else:
		message = "null"
		message = step["step"][0]["direction"]
		result.update({'message' : message})
	
	message = step["step"][0]["direction"]
	result = {"message" : str(message)}
	return json.loads(json.dumps(result))

class get_distance_time(Resource):
	def get(self, lat1, lng1, loc):
		try:
			loc = str(loc)
			loc = loc.replace(' ','+')
			url1 = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input="+loc+"&key=AIzaSyA4USvlgWp3Fui5JSeLMIpeCQBJDq_uw0U"
			res = requests.get(url1)
			res = res.json()
			place_id = res["predictions"][0]["place_id"]
			url2 = "https://maps.googleapis.com/maps/api/place/details/json?placeid="+place_id+"&key=AIzaSyA4USvlgWp3Fui5JSeLMIpeCQBJDq_uw0U"
			res = requests.get(url2)
			res = res.json()
			res = res["result"]
			lat2, lng2 = str(res["geometry"]["location"]["lat"]), str(res["geometry"]["location"]["lng"])
			lat2 = lat2.decode('utf-8')
			lng2 = lng2.decode('utf-8')

			feed_steps(lat1, lng1, lat2, lng2)

			distance, time = calculate_distance_time(lat1, lng1, lat2, lng2)
			result = dict()
			result.update({"info" : [{"distance" : str(distance)+"metres"}, {"duration" : str(time)+"seconds"}]})
			#with open('info.json','w') as fp:
			#	json.dump(result, fp)
			return json.loads(json.dumps(result))
			#return Response(None, mimetype='application/json', headers={'Content-Disposition':'attachment;filename=info.json'})
		except:
			pass

class get_status(Resource):
	def get(self, lat, lng):
		return get_current_status(lat, lng)

api.add_resource(get_distance_time,"/info/start"+"="+"<string:lat1>,<string:lng1>&end"+"="+"<string:loc>")
api.add_resource(get_status,"/status/start"+"="+"<string:lat>,<string:lng>")

if(__name__=='__main__'):
	app.run(host="0.0.0.0",port=8000,debug=True)