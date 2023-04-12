import csv
import json

translationDict = {
    "고백, 꽃, 늑대": "Proposed, Flower, Wolf",
    "고백, 꽃, 늑대 part.2": "Proposed, Flower, Wolf part.2",
    "공성전 ~Pierre Blanche, Yonce Remix~": "The Siege warfare ~Pierre Blanche, Yonce Remix~",
    "광명의 루 : 루 라바다": "Lugh Lamhfada",
    "꿈의 기억 (feat. Jisun)": "Memories of dreams",
    "내게로 와": "Come to me",
    "너랑 있으면": "To Be With You",
    "너로피어오라": "Flowering",
    "너로피어오라 ~Original Ver.~": "Flowering ~Original Ver.~",
    "너에게": "To You",
    "느낌": "Feel [EZ2]",
    "모차르트 교향곡 40번 1악장": "Mozart Symphony No.40 1st Mvt.",
    "바람에게 부탁해": "Ask to Wind",
    "바람에게 부탁해 ~Live Mix~": "Ask to Wind ~Live Mix~",
    "바람의 기억": "Memory of Wind",
    "밤비 (Bambi) - DJMAX Edit -": "Bambi - DJMAX Edit -",
    "별빛정원": "Starlight Garden",
    "복수혈전": "REVENGE",
    "부여성 ~Blosso Remix~": "Buyeo Fortress ~Blosso Remix~",
    "비상 ~Stay With Me~": "Soar ~Stay With Me~",
    "서울여자": "SOUL LADY",
    "설레임": "HeartBeat",
    "설레임 Part.2": "Heart Beat Part.2",
    "소년 모험가 ~SiNA Remix~": "Young Adventurer ~SiNA Remix~",
    "아침형 인간": "Every Morning",
    "안아줘": "pit-a-pet",
    "어릴 적 할머니가 들려주신 옛 전설": "An old story from Grandma",
    "연합군과 제국군": "Alliance x Empire",
    "염라": "Karma",
    "영원": "Forever",
    "유령": "Ghost",
    "전설이 시작된 곳 ~VoidRover Remix~": "Where Legend Begin ~VoidRover Remix~",
    "죽음의 신 : 크로우 크루아흐": "Cromm Cruaich",
    "최종무곡": "The Final Dance",
    "카트라이더 Mashup ~Cosmograph Remix~": "Kartrider Mashup ~Cosmograph Remix~",
    "카트라이더 Mashup ~Pure 100% Remix~": "Kartrider Mashup ~Pure 100% Remix~",
    "카트라이더, 크레이지아케이드, 버블파이터 Main theme ~CHUCK Remix~": "Kartrider, Crazyarcade, Bubblefighter Main theme ~CHUCK Remix~",
    "태권부리": "Taekwonburi",
    "피아노 협주곡 1번": "Piano Concerto No. 1",
    "혜성": "comet",
    "Eternal Fantasy ~유니의 꿈~": "Eternal Fantasy",
    "Eternal Memory ~소녀의 꿈~": "Eternal Memory",
    "I want You ~반짝★반짝 Sunshine~": "I want You ~Twinkle Twinkle Sunshine~",
    "NB RANGERS - 운명의 Destiny": "NB RANGERS - Destiny"
}

artistDict = {
    "김동현": "Kim Dong Hyun",
    "김창훈": "Kim Changhoon",
    "정영걸": "Jung Young Gul",
    "이궐": "Lee Geol",
}

def diffToBool(song):
    # Every song in Respect has a NM chart for each mode, so those are set to true by default
    song["4BNM"] = True
    song["5BNM"] = True
    song["6BNM"] = True
    song["8BNM"] = True

    # Now we check if the other difficulties exist for that song and convert as appropriate
    # AllTrackData.csv considers a difficulty to not exist if that col value is set to 0
    diffFilter = ("4BHD", "4BMX", "4BSC", "5BHD", "5BMX", "5BSC", "6BHD", "6BMX", "6BSC", "8BHD", "8BMX", "8BSC")
    filteredSongDiffs = {diff : song[diff] for diff in diffFilter}
    
    for diff in filteredSongDiffs:
        if filteredSongDiffs[diff] == "0":
            song[diff] = False
        else:
            song[diff] = True

def renameKeys(song):
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
    
    # Translate KR titles + artists to their EN equivalents
    # We can loop through the entire song list and swap the Korean entries to their corresponding translation in translationDict
    # This is independent of entry ordering inside AllTrackData.csv
    for song in toConvert:
        krTitle = song['Title']
        krArtist = song['Artist']
        if krTitle in translationDict:
            song['Title'] = translationDict[krTitle]
        if krArtist in artistDict:
            song['Artist'] = artistDict[krArtist]

        # Next, convert all (non)-existent difficulties to bools
        diffToBool(song)

        # And regenerate the difficulty keys
        renameKeys(song)

    # Write all changes to JSON
    with open(jsonFilePath, 'w', encoding='utf-8-sig') as jsonOut:
        json.dump(toConvert, jsonOut, indent=4)

csvFile = 'AllTrackData.csv'
jsonFile = 'SongData.json'
csvToJson(csvFile, jsonFile)
conversion(jsonFile)

exit()