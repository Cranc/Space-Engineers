var dict;

void Main()
{
    refinery_list = new List<IMyTerminalBlock>();
	GridTerminalSystem.GetBlocksOfType<IMyRefinery>(refinery_list);


    dict = new Dictionary<string, MyFixedPoint>();

	for (int i = 0; i < refinery_list.Count; i++ ){
        AddToDictionary(refinery_list[i].CustomName, GetMassOfInventory(refinery_list[i]));
	}
}

MyFixedPoint GetMassOfInventory(IMyTerminalBlock MyBlock)
{
    //toDo go through all inventorys and sum up item Mass
    //var item_list = new List<IMyInventoryItem>();
    //item_list = MyBlock.GetInventory().GetItems();
}

///<param name="item"> name of key
///<param name="inv"> inventory
void AddToDictionary(string name, MyFixedPoint amount)
{
    if(!dict.ContainsKey(name))
    {
         dict.Add(name, amount);
    }
    dict[name] = amount
}