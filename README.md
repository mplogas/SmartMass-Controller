# SmartMass Controller
A web-based interface for [SmartMass](https://github.com/mplogas/SmartMass) devices, allows configuration of the devices and offers an inventory UI. It is focussed on 3D printing filament but can be easily adapted for other means.   

## Device features
- Discovery of unknown devices
- Initialize calibration
- Create and send configuration to the scale
- Start tare process
- Read device messages

## Inventory features
- Manage materials and manufacturers
- Take inventory of your filament spools
- Write spool data to RFID tags (via device)
- store last weighing result

## Tech
- aspnet 6.0 (webapi)
- hosted blazor webassembly
- signalR
- MQTTnet
- EFCore / SQLite
