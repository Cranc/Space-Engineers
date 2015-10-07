Dictionary<IMyTerminalBlock, double> dict;
List<KeyValuePair<IMyTerminalBlock,  double>> ref_list;

void Main()
{
    var refinery_list = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyRefinery>(refinery_list);

    var debug_list = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyBeacon>(debug_list);
    debug_list[0].SetCustomName("Debug Mode");

    dict = new Dictionary<IMyTerminalBlock, double>();
    ref_list = new List<KeyValuePair<IMyTerminalBlock,  double>>();
	for (int i = 0; i < refinery_list.Count; i++ ){
        //AddToDictionary(refinery_list[i], GetVolumeOfInventory(refinery_list[i]));
        debug_list[0].SetCustomName(debug_list[0].CustomName + "max volume as string: " + refinery_list[i].GetInventory(0).CurrentVolume.ToString() + " ... ");
        var test = VRage.MyFixedPoint.DeserializeStringSafe(refinery_list[i].GetInventory(0).CurrentVolume.ToString());
        debug_list[0].SetCustomName(debug_list[0].CustomName + "DeserializedString: " + test + " ... ");
        ref_list.Add(new KeyValuePair<IMyTerminalBlock,  double>(refinery_list[i], GetVolumeOfInventory(refinery_list[i])));
	}

	List<KeyValuePair<IMyTerminalBlock,  double>> mySortList = ref_list;

    if( mySortList != null) {
        debug_list[0].SetCustomName(debug_list[0].CustomName + "sort list not empty ...");
        mySortList.Sort((x,y)=>y.Value.CompareTo(x.Value));
        int end = mySortList.Count - 1;
        for (int start = 0; start < end; start++){
            var dif = mySortList[start].Value - mySortList[end].Value;
            dif /= 2;
            var inv_one = mySortList[start].Key.GetInventory(0);
            var inv_two = mySortList[end].Key.GetInventory(0);

            debug_list[0].SetCustomName(debug_list[0].CustomName + "difference as double: " + dif + "...");
            debug_list[0].SetCustomName(debug_list[0].CustomName + "difference as MyFixedPoint: " + (VRage.MyFixedPoint)dif + "...");

            if( inv_one.IsConnectedTo(inv_two)) {
                debug_list[0].SetCustomName(debug_list[0].CustomName + "inventory is connected ...");
                var itemList = new List<IMyInventoryItem>();
                mySortList[start].Key.GetInventory(0).GetItems();

                var theresult = mySortList[start].Key.GetInventory(0).TransferItemTo(
                    mySortList[end].Key.GetInventory(0),
                    0,
                    stackIfPossible: true,
                    amount: (VRage.MyFixedPoint)dif
                );

                debug_list[0].SetCustomName(debug_list[0].CustomName + "Transfer sucessful: " + theresult + "...");
            }else{
                debug_list[0].SetCustomName(debug_list[0].CustomName + "inventory not connected ...");
            }
            end--;
        }
    }else{
        debug_list[0].SetCustomName(debug_list[0].CustomName + "sort list empty ...");
    }
}

double GetVolumeOfInventory(IMyTerminalBlock myref)
{
    //toDo go through all input inventorys and sum up item Mass
    var input_inv = myref.GetInventory(0);

    return (double)(input_inv.MaxVolume - input_inv.CurrentVolume);
}

///<param name="item"> name of key
///<param name="inv"> inventory
void AddToDictionary(IMyTerminalBlock name,  double amount)
{
    if(!dict.ContainsKey(name))
    {
         dict.Add(name, amount);
    }
    dict[name] = amount;
}

void AddToList(ref IMyTerminalBlock name,  double amount){
    ref_list.Add(new KeyValuePair<IMyTerminalBlock,  double>(name, amount));
}

















List<KeyValuePair<IMyTerminalBlock,  double>> MyToList(Dictionary<IMyTerminalBlock, double> dict){
    var myList = new List<KeyValuePair<IMyTerminalBlock, double>>();

    for(int i=0; i<dict.Count; i++){
        myList.Add(new KeyValuePair<IMyTerminalBlock, double>(dict[i].Value,dict[i].Value));
    }
    return myList;
}









//// STUFF
	/*mySortList.Sort(
        delegate(KeyValuePair<IMyTerminalBlock, double> firstPair,
        KeyValuePair<IMyTerminalBlock, double> nextPair)
        {
            return firstPair.Value.CompareTo(nextPair.Value);
        }
    );*/

                //var ser_def = new SerializableDefinitionId(itemList[0].Content.TypeId,itemList[0].Content.SubtypeName);
                /*if(mySortList[end].Key.GetInventory(0).CanItemsBeAdded((VRage.MyFixedPoint)dif,ser_def)) {
                        mySortList[start].Key.GetInventory(0).TransferItemTo(
                        mySortList[end].Key.GetInventory(0),
                        0,
                        stackIfPossible: true,
                        amount: dif

                    );*/