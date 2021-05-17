import csv
import json

translationDict = {
    "고백, 꽃, 늑대": "Proposed, Flower, Wolf",
    "고백, 꽃, 늑대 part.2": "Proposed, Flower, Wolf part.2",
    "내게로 와": "Come to me",
    "너에게": "To You",
    "바람에게 부탁해": "Ask to Wind",
    "바람에게 부탁해 ~Live Mix~": "Ask to Wind ~Live Mix~",
    "바람의 기억": "Memory of Wind",
    "비상 ~Stay With Me~": "Soar ~Stay With Me~",
    "설레임": "HeartBeat",
    "설레임 Part.2": "Heart Beat Part.2",
    "아침형 인간": "Every Morning",
    "염라": "Karma",
    "영원": "Forever",
    "유령": "Ghost",
    "태권부리": "Taekwonburi",
    "피아노 협주곡 1번": "Piano Concerto No. 1",
    "혜성": "comet",
    "Eternal Fantasy ~유니의 꿈~": "Eternal Fantasy",
    "Eternal Memory ~소녀의 꿈~": "Eternal Memory",
    "I want You ~반짝★반짝 Sunshine~": "I want You ~Twinkle Twinkle Sunshine~"
}

def diffToBool(convert):
    for song in convert:
        # Every song in Respect has a NM chart for each mode, so those are set to true by default
        song["4BNM"] = True
        song["5BNM"] = True
        song["6BNM"] = True
        song["8BNM"] = True

        # Now we check if the other difficulties exist for that song and convert as appropriate
        # AllTrackData.csv considers a difficulty to not exist if that col value is set to 0
        # 4B
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
        
        # 5B
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
        
        # 6B
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
        
        # 8B
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
        # Pop all difficulty keys so they don't start with a number
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
      
    # Open csv
    with open(csvFilePath, encoding='utf-8-sig') as csvf: 
        # Load file via DictReader
        csvReader = csv.DictReader(csvf) 

        # Convert csv rows into python dict
        for row in csvReader: 
            # Add dict to jsonArray
            jsonArray.append(row)
  
    # Convert jsonArray to JSON String and write to file
    with open(jsonFilePath, 'w', encoding='utf-8-sig') as jsonf: 
        jsonString = json.dumps(jsonArray, indent=4)
        jsonf.write(jsonString)

def conversion(jsonFilePath):
    # Open KR json and load to var
    with open(jsonFilePath, encoding='utf-8-sig') as jsonf:
        toConvert = json.load(jsonf)
    
    # Translate KR titles to their EN equivalents
    # We can loop through the entire song list and swap KR titles to their corresponding translation in translationDict
    # This is independent of entry ordering and shouldn't change much even with Technika Q's Starlight Garden
    # Note that this is completely reliant on DJMAX Random Selector's AllTrackData.csv entries
    for song in toConvert:
        krTitle = song['Title']
        if krTitle in translationDict:
            song['Title'] = translationDict[krTitle]

    # Next, convert all (non)-existent difficulties to bools
    diffToBool(toConvert)

    # And regenerate the difficulty keys
    renameKeys(toConvert)

    # Write all changes to JSON
    with open(jsonFilePath, 'w', encoding='utf-8-sig') as jsonOut:
        json.dump(toConvert, jsonOut, indent=4)

csvFile = 'AllTrackData.csv'
jsonFile = 'SongData.json'
csvToJson(csvFile, jsonFile)
conversion(jsonFile)

exit()