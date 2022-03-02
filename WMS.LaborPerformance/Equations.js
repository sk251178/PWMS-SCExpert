//pickup time
if([CatchWeight]==true){[PickConstant]+[PickVariable]*[PickWeight]+[CatchWeightCaseConstant]*Math.ceil([PickQuantity]/[GrabFactor])/1000}else{[PickConstant]+[PickVariable]*[PickWeight]*Math.ceil([PickQuantity]/[GrabFactor])/1000};

//route cost
  if ([vehType]==null){[_DefCostPerDay]+[_DefCostPerDistUnit] * [tmpDist]+[tmpDrvTime]*[_DefCostPerHour]}else
                      {[vehTypeCOSTPERDAY] + [vehTypeCOSTPERDISTANCEUINIT]*[tmpDist]+[tmpDrvTime]*[vehTypeCOSTPERHOUR]};

/////////test/////////servicetime
//numStop
//numTask
//TaskVolume
//TaskWeight
//TaskType
//TaskValue
//CompanyServiceTime
//StrategyServiceTime
//ServiceTime - default=0

if([TaskWeight]>40){20}else{10};

/////////////routecost
//VehType
//DefCostPerDay
//DefCostPerHour
//DefCostPerDistUnit
//TotalDist
//TotalDrvTime
//VehCostPerDay
//VehCostPerHour
//VehCostPerDistanceUnit
//VehCostPerDistanceUnit
//RouteCost - default=0

if([VehType]==0){[DefCostPerDay]+[DefCostPerDistUnit]*[TotalDist]/1000+[DefCostPerHour]*[TotalDrvTime]/3600}else{[VehCostPerDay]+[VehCostPerDistanceUnit]*[TotalDist]/1000+[VehCostPerHour]*[TotalDrvTime]/3600};

//DOCUMENTTIMECALCULATIONMETHODS - Receipt
if([Receipt]!=null){[numSkus]+[numLines]+[numPO]+[Weight]+[Volume]+[numPallets]+[NumCatchWeight]}else{0}





