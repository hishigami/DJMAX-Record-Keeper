import csv
import json

def diffToBool(convert):
    for song in convert:
        #Every song in Respect has a NM chart for each mode, so those are set to true by default
        song["4BNM"] = True
        song["5BNM"] = True
        song["6BNM"] = True
        song["8BNM"] = True

        #Now we check if the other difficulties exist for that song and convert as appropriate
        #AllTrackData.csv considers a difficulty to not exist if that col value is set to 0
        #4B
        if(song["4BHD"] == "0"):
            song["4BHD"] = False
        else:
            song["4BHD"] = True
        if(song["4BMX"] == "0"):
            song["4BMX"] = False
        else:
            song["4BMX"] = True
        if(song["4BSC"] == "0"):
            song["4BSC"] = False
        else:
            song["4BSC"] = True
        
        #5B
        if(song["5BHD"] == "0"):
            song["5BHD"] = False
        else:
            song["5BHD"] = True
        if(song["5BMX"] == "0"):
            song["5BMX"] = False
        else:
            song["5BMX"] = True
        if(song["5BSC"] == "0"):
            song["5BSC"] = False
        else:
            song["5BSC"] = True
        
        #6B
        if(song["6BHD"] == "0"):
            song["6BHD"] = False
        else:
            song["6BHD"] = True
        if(song["6BMX"] == "0"):
            song["6BMX"] = False
        else:
            song["6BMX"] = True
        if(song["6BSC"] == "0"):
            song["6BSC"] = False
        else:
            song["6BSC"] = True
        
        #8B
        if(song["8BHD"] == "0"):
            song["8BHD"] = False
        else:
            song["8BHD"] = True
        if(song["8BMX"] == "0"):
            song["8BMX"] = False
        else:
            song["8BMX"] = True
        if(song["8BSC"] == "0"):
            song["8BSC"] = False
        else:
            song["8BSC"] = True

def renameKeys(rename):
    for song in rename:
        #Pop all difficulty keys so they don't start with a number
        song["FourNM"] = song.pop("4BNM")
        song["FourHD"] = song.pop("4BHD")
        song["FourMX"] = song.pop("4BMX")
        song["FourSC"] = song.pop("4BSC")
        song["FiveNM"] = song.pop("5BNM")
        song["FiveHD"] = song.pop("5BHD")
        song["FiveMX"] = song.pop("5BMX")
        song["FiveSC"] = song.pop("5BSC")
        song["SixNM"] = song.pop("6BNM")
        song["SixHD"] = song.pop("6BHD")
        song["SixMX"] = song.pop("6BMX")
        song["SixSC"] = song.pop("6BSC")
        song["EightNM"] = song.pop("8BNM")
        song["EightHD"] = song.pop("8BHD")
        song["EightMX"] = song.pop("8BMX")
        song["EightSC"] = song.pop("8BSC")


def csvToJson(csvFilePath, jsonFilePath):
    jsonArray = []
      
    #Open csv
    with open(csvFilePath, encoding='utf-8-sig') as csvf: 
        #Load file via DictReader
        csvReader = csv.DictReader(csvf) 

        #Convert csv rows into python dict
        for row in csvReader: 
            #Add dict to jsonArray
            jsonArray.append(row)
  
    #Convert jsonArray to JSON String and write to file
    with open(jsonFilePath, 'w', encoding='utf-8-sig') as jsonf: 
        jsonString = json.dumps(jsonArray, indent=4)
        jsonf.write(jsonString)

def conversion(jsonFilePath):
    #Open KR json and load to var
    with open(jsonFilePath, encoding='utf-8-sig') as jsonf:
        toConvert = json.load(jsonf)
    
    #Translate KR titles to their EN equivalents
    #Instead of looping through the entire JSON and subsituting exact matches for gibberish substrings spat out from the above function,
    #we can "brute force" translate by index given Respect's alphabetical/hangul title sort; there are only so many hangul titles in DJMAX as a whole
    #This *generally* works well for the hangul titles; at worst, only up to 3 of the last few songs' indices need to be updated every song patch
    #The only forseeable issue that'd affect the hangul songs as well is Technika Q's Starlight Garden
    #This is also completely reliant on AllTrackData.csv's own sort from DJMAX Random Selector to begin with
    toConvert[0]['Title'] = "Proposed, Flower, Wolf"
    toConvert[1]['Title'] = "Proposed, Flower, Wolf part.2"
    toConvert[2]['Title'] = "Come to me"
    toConvert[3]['Title'] = "To You"
    toConvert[4]['Title'] = "Ask to Wind"
    toConvert[5]['Title'] = "Ask to Wind ~Live Mix~"
    toConvert[6]['Title'] = "Memory of Wind"
    toConvert[7]['Title'] = "Soar ~Stay With Me~"
    toConvert[8]['Title'] = "HeartBeat"
    toConvert[9]['Title'] = "Heart Beat Part.2"
    toConvert[10]['Title'] = "Every Morning"
    toConvert[11]['Title'] = "Karma"
    toConvert[12]['Title'] = "Forever"
    toConvert[13]['Title'] = "Ghost"
    toConvert[14]['Title'] = "Taekwonburi"
    toConvert[15]['Title'] = "Piano Concerto No. 1"
    toConvert[16]['Title'] = "comet"
    toConvert[118]['Title'] = "Eternal Fantasy"
    toConvert[119]['Title'] = "Eternal Memory"
    toConvert[179]['Title'] = "I want You ~Twinkle Twinkle Sunshine~"

    #Next, convert all (non)-existent difficulties to bools
    diffToBool(toConvert)

    #And regenerate the difficulty keys
    renameKeys(toConvert)

    #Write all changes to JSON
    with open(jsonFilePath, 'w', encoding='utf-8-sig') as jsonOut:
        json.dump(toConvert, jsonOut, indent=4)

csvFile = 'AllTrackData.csv'
jsonFile = 'SongData.json'
csvToJson(csvFile, jsonFile)
conversion(jsonFile)

exit()