import aamp, sys, os, json

def getParam(act):
    return act.split("(")[1].split(")")[0]

def Parse(pio, instructions):
    for instruction in instructions.split(";"):
        currentObj = pio

        for action in instruction.split("."):
            if action.startswith("l"):
                currentObj = currentObj.list(getParam(action))
            elif action.startswith("o"):
                currentObj = currentObj.object(getParam(action))
            elif action.startswith("v"):
                params = getParam(action).split(",")
                currentObj.set_param(params[0], params[1])

if __name__ == "__main__":
    with open(os.getenv("APPDATA") + "\\BOTWM\\Temp\\AampTemp.txt", 'r') as f:
        AampData = json.loads(f.read())

    result = []

    for aampFile in AampData:
        pio = aamp.Reader(bytearray.fromhex(aampFile["Data"])).parse()
        Parse(pio, aampFile["Instruction"])
        result.append(' '.join(format(x, '02x') for x in aamp.Writer(pio).get_bytes()))

    with open(os.getenv("APPDATA") + "\\BOTWM\\Temp\\AampTemp.txt", 'w') as f:
        f.write(json.dumps(result, indent= 4))