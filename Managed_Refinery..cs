var dict;

void Main()
{
    refinery_list = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyRefinery>(refinery_list);


    dict = new Dictionary<IMyRefinery, MyFixedPoint>();

	for (int i = 0; i < refinery_list.Count; i++ ){
        AddToDictionary(refinery_list[i], GetVolumeOfInventory(refinery_list[i]));
	}

	List<KeyValuePair<IMyRefinery, MyFixedPiont>> mySortList = dict.ToList();

	/*mySortList.Sort(
        delegate(KeyValuePair<IMyRefinery, MyFixedPiont> firstPair,
        KeyValuePair<IMyRefinery, MyFixedPiont> nextPair)
        {
            return firstPair.Value.CompareTo(nextPair.Value);
        }
    );*/
    mySortList.Sort((x,y)=>y.Value.CompareTo(x.Value));
    int start = 0;
    int end = mySortList.Count;
    for (start < end; start++){
        var dif = mySortList[start] - mySortList[end];
        dif /= 2;
        if(mySortList[start].Key.GetInventory(0).IsConnectedTo(mySortList[end].Key.GetInventory(0)) {
            var itemList = new List<IMyInventoryItem>;
            mySortList[start].Key.GetInventory(0).GetItems();
            if(mySortList[end].Key.GetInventory(0).CanItemsBeAdded(dif,itemList[0])) {
                    mySortList[start].Key.GetInventory(0).TransferItemTo(
                    mySortList[end].Key.GetInventory(0),
                    0,
                    stackIfPossible: true,
                    amount: dif

                );
            }
        }
        end--;
    }
}

MyFixedPoint GetVolumeOfInventory(IMyRefinery myref)
{
    //toDo go through all input inventorys and sum up item Mass
    var input_inv = myref.GetInventory(0);
    return input_inv.MaxVolume - input_inv.CurrentVolume();
}

///<param name="item"> name of key
///<param name="inv"> inventory
void AddToDictionary(IMyRefinery name, MyFixedPoint amount)
{
    if(!dict.ContainsKey(name))
    {
         dict.Add(name, amount);
    }
    dict[name] = amount
}