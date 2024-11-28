var originalOrder = new FoodOrder(
  info: new OrderInfo(1),
  customerName: "John Doe",
  isDelivery: true,
  contents: ["Pizza", "Coke"]
);
originalOrder.Debug();

var copiedOrder = (FoodOrder)originalOrder.ShallowCopy();
copiedOrder.Debug();

copiedOrder.OrderInfo.Id = 2;
copiedOrder.CustomerName = "Jane Doe";

originalOrder.Debug();
copiedOrder.Debug();

var manager = new PrototypeManager();
manager["pizza"] = new FoodOrder(
  info: new OrderInfo(1),
  customerName: "John Doe",
  isDelivery: true,
  contents: ["Pizza", "Coke"]
);

var copiedPizza = (FoodOrder)manager["pizza"].DeepCopy();

copiedPizza.Debug();

abstract class Prototype
{
  public abstract void Debug();
  public abstract Prototype ShallowCopy();
  public abstract Prototype DeepCopy();
}

class OrderInfo(int id)
{
  public int Id { get; set; } = id;
}

class FoodOrder(OrderInfo info, string customerName, bool isDelivery, string[] contents) : Prototype
{
  public OrderInfo OrderInfo { get; set; } = info;
  public string CustomerName { get; set; } = customerName;
  public bool IsDelivery { get; set; } = isDelivery;
  public string[] Contents { get; set; } = contents;

  public override void Debug()
  {
    Console.WriteLine("------- Order Details ------");
    Console.WriteLine($"Order ID: {OrderInfo.Id}");
    Console.WriteLine($"Customer Name: {CustomerName}");
    Console.WriteLine($"Delivery: {IsDelivery}");
    Console.WriteLine("Contents:");
    
    foreach (var item in Contents)
    {
      Console.WriteLine($"- {item}");
    }
  }

  // this seems tedious...so i've got to implement
  // a specific deep copy method for each class
  // and manually handle each property that is a reference type?
  // nah...there is most definitely a way to do this with reflection
  // better yet I'll bet there is a nuget package that does this
  public override Prototype DeepCopy()
  {
    var clonedOrder = (FoodOrder)MemberwiseClone();
    clonedOrder.OrderInfo = new OrderInfo(OrderInfo.Id);

    return clonedOrder;
  }

  // value types are copied by value
  // reference types are copied by reference
  // this means that any changes to the copied object
  // will affect the original object if it is a reference type
  public override Prototype ShallowCopy() => (Prototype)MemberwiseClone();
}

// works well with Object Pool...not sure though
// if I see the value in this additional manager class.
class PrototypeManager
{
  private readonly Dictionary<string, Prototype> _prototypes = [];

  public Prototype this[string key]
  {
    get => _prototypes[key];
    set => _prototypes[key] = value;
  }
}
