This project is a simple inventory and order management system integrated with a Universal Robots (UR) simulator.
It automatically processes customer orders and moves a simulated robot arm to pick up and place items into a shipment box.

Features:
Inventory and order tracking
Automatic order processing
Robot control using URScript via TCP
Works with URSim (Universal Robots simulator via Docker)
Avalonia GUI with live status updates
 
Project Structure
Models/
Item, BulkItem, UnitItem - represent inventory items
Order, OrderLine, OrderBook — manage customer orders
Inventory — tracks stock levels
Customer — creates and queues orders
Robot, ItemSorterRobot — handle URScript communication
ViewModels/
MainWindowViewModel — core logic connecting UI and robot
ViewModelBase — base class for data binding

How to Run
Start the UR simulator (URSim) in Docker and open:
http://localhost:6080/vnc.html
Set the robot to Local Control and press Play.
Run the Avalonia GUI project.
Click “Process Order” to see the robot move and pick items.

Requirements
.NET 8.0
Avalonia UI
URSim Docker container (ports 29999 and 30002 exposed)
Notes
The robot works entirely in simulation — no real hardware required.
Each movement takes about 9.5 seconds per item.
URScript uses invariant culture formatting (. decimal separator).
