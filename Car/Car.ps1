Add-Type -Path D:\Car\Car\bin\Debug\Car.dll

$car1 = New-Object My.Car
$car1.Manufacturer = 'VW'
$car1.Color = 'red'
$car1.Model = 'Golf'
$car1.Accelerate(50)
$car1.Accelerate(30)
$car1
