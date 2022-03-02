Public Class TABLES
    Public Const MENU As String = "rad_menu"
    Public Const WAREHOUSES As String = "WAREHOUSES"
    Public Const USERPROFILE As String = "USERPROFILE"
    Public Const USERWAREHOUSE As String = "USERWAREHOUSE"
    Public Const SYSTEMPARAMETERS As String = "SYSTEMPARAMETERS"
End Class

Public Class MSMQUEUES
    Public Const AUDIT As String = "Audit"
    Public Const UPLOADER As String = "Uploader"
    Public Const SHIPPER As String = "Shipper"
    Public Const LABORSYNC As String = "LaborPerformanceSyncProc"
    Public Const LABOR As String = "LaborPerformanceProc"
End Class

Public Class TASKSUBTYPE
    Public Const BULKHEADINSTAL As String = "BLKHDINSTL"
End Class

Public Class DOCUMENTTYPES
    Public Const OUTBOUNDORDER As String = "OUTBOUND"
    Public Const FLOWTHROUGH As String = "FLWTH"
    Public Const TRANSHIPMENT As String = "TRANSSHIP"
    Public Const INBOUNDORDER As String = "INBOUND"
    Public Const ROUTETASK As String = "ROUTETASK"
    Public Const RECEIPT As String = "RECEIPT"
    Public Const SHIPMENT As String = "SHIPMENT"
End Class

Public Class ATTACHMENTTYPES
    Public Const IMAGE As String = "IMAGE"
    Public Const PDF As String = "PDF"
    Public Const WORDDOC As String = "WORDDOC"
    Public Const EXCELDOC As String = "EXCELDOC"
End Class

Public Class SESSION
    Public Const SHOWTASKMANAGER As String = "ShowedTaskManager"
    Public Const BATCHREPLENLETDOWNTASK As String = "BatchReplenishmentLetdownTask"
    Public Const BATCHREPLENUNLOADTASK As String = "BatchReplenishmentUnloadTask"
    Public Const BATCHREPLENLETDOWNHEADER As String = "BatchReplenishmentLetdownHeader"
    Public Const BATCHREPLENUNLOADHEADER As String = "BatchReplenishmentUnloadHeader"
    Public Const BATCHREPLENDETAILCOLLLECTION As String = "BatchReplenDetailCollection"
    Public Const BATCHREPLENHEADERPRINTER As String = "BatchReplenishmentHeaderPrinter"

End Class

Public Class VIEWS

End Class

Public Class CONFIRMATIONTYPE
    Public Const NONE As String = "NONE"
    Public Const LOCATION As String = "LOCATION"
    Public Const LOAD As String = "LOAD"
    Public Const UPC As String = "UPC"
    Public Const SKU As String = "SKU"
    Public Const SKULOCATION As String = "SKULOC"
    Public Const SKULOCATIONUOM As String = "SKULOCUOM"
    Public Const SKUUOM As String = "SKUUOM"
End Class


Public Class USERS
    Public Const SYSTEMUSER As String = "SYSTEM"
    Public Const PLANNER As String = "PLANNER"
End Class

Public Class TASKTYPE
    Public Const REPLENISHMENT As String = "REPL"
    Public Const FULLREPL As String = "FULLREPL"
    Public Const NEGTREPL As String = "NEGTREPL"
    Public Const PARTREPL As String = "PARTREPL"
    Public Const LOADCOUNTING As String = "LDCOUNT"
    Public Const LOCATIONCOUNTING As String = "LOCCNT"
    Public Const LOCATIONBULKCOUNTING As String = "LOCBLKCNT"
    Public Const PARTIALPICKING As String = "PARPICK"
    Public Const NEGPALLETPICK As String = "NPP"
    Public Const FULLPICKING As String = "FULLPICK"
    Public Const PICKING As String = "PICKING"
    Public Const PARALLELPICKING As String = "PARALLELPK"
    Public Const LOADPUTAWAY As String = "LOADPW"
    Public Const CONTPUTAWAY As String = "CONTPW"
    Public Const CONTLOADPUTAWAY As String = "CONTLDPW"
    Public Const LOADDELIVERY As String = "LOADDEL"
    Public Const CONTDELIVERY As String = "CONTDEL"
    Public Const CONTLOADDELIVERY As String = "CONTLDDEL"
    Public Const CONTCONTDELIVERY As String = "CONTCONDEL"
    Public Const LOADLOADING As String = "LOADLOAD"
    Public Const CONTLOADING As String = "CONTLOAD"
    Public Const CASELOADING As String = "CASELOAD"
    Public Const CONTLOADLOADING As String = "CONTLDLOAD"
    Public Const CONTCONTLOADING As String = "CONTCONLOAD"
    Public Const PUTAWAY As String = "PUTAWAY"
    Public Const DELIVERY As String = "DELIVERY"
    Public Const CONSOLIDATION As String = "CONSOLID"
    Public Const CONSOLIDATIONDELIVERY As String = "CONSOLDEL"
    Public Const EMPTYHUPICKUPTASK As String = "EMPHUDEL"
    Public Const UNLOADING As String = "UNLOADING"
    Public Const NSPICKUP As String = "NSPICKUP"
    Public Const SPICKUP As String = "SPICKUP"
    Public Const MISC As String = "MISC"
    'Added for PWMS-756 for Short Pick
    Public Const SHORTPICK As String = "SHORTPICK"
    Public Const BRUNLOAD As String = "BRUNLOAD"
    Public Const BRLETDOWN As String = "BRLETDOWN"

End Class

Public Class TASKASSIGNTYPE
    Public Const MANUAL As String = "MANUAL"
    Public Const AUTOMATIC As String = "AUTOMATIC"
End Class
Public Class TASKPRIORITY
    Public Const PRIORITY_PENDING As String = "PENDING"
    Public Const PRIORITY_NORMAL As String = "NORMAL"
    Public Const PRIORITY_IMMEDIATE As String = "IMMEDIATE"
    Public Const PRIORITY_ORDER_DEMAND As String = "ORDER_DEMAND"

End Class
Public Class INVENTORY
    Public Const ADDQTY As String = "ADDQTY"
    Public Const SUBQTY As String = "SUBQTY"
    'RWMS-1483 and RWMS-1250
    Public Const NEWQTY As String = "NEWQTY"
    'End RWMS-1483 and RWMS-1250
    Public Const CANCELLOAD As String = "CANLOAD"
    Public Const SPLITLOAD As String = "SPLITLOAD"
    Public Const CHANGEUOM As String = "CHANGEUOM"
    Public Const CHANGESKU As String = "CHANGESKU"
    Public Const CREATELOAD As String = "CREATELOAD"
    Public Const UNRECEIVELOAD As String = "UNRCVLOAD"
    Public Const WOLDCNSM As String = "WOLDCNSM"
    Public Const WOLDPRDC As String = "WOLDPRDC"
    Public Const LDATTIN As String = "LDATTIN"
    Public Const LDATTOUT As String = "LDATTOUT"
End Class

Public Class LoadActivityTypes
    Public Const PACKING As String = "PACKING"
    Public Const UNPACKING As String = "UNPACKING"
    Public Const PICKING As String = "PICKING"
    Public Const LOADING As String = "LOADING"
    Public Const STAGING As String = "STAGING"
    Public Const SHIPPING As String = "SHIPPING"
    Public Const VERIFYING As String = "VERIFYING"
End Class

Public Class REPORTS
    Public Const RECEIVINGWORKSHEET As String = "Receiving Worksheet"
    Public Const RECEIVINGMANIFEST As String = "Receiving Manifest"
    Public Const SHIPPINGMANIFEST As String = "Shipping Manifest"
    Public Const PICKLISTS As String = "PickList"
    Public Const LOADLOCATIONS As String = "LoadLocations"
End Class

Public Class REPORTACTIONTYPE
    Public Const SAVE As String = "SAVE"
    Public Const PRINT As String = "PRINT"
End Class

Public Class FLOWTHROUGHLABELTYPE
    Public Const NONE As String = "NONE"
    Public Const FLOWTHROUGH As String = "FLOWTHROUGH"
End Class

Public Class TRANSSHIPMENTLABELTYPE
    Public Const NONE As String = "NONE"
    Public Const TRANSSHIPMENT As String = "TRANSSHIPMENT"
End Class

Public Class LOCATIONACCESSTYPE
    Public Const RANDOMACCESS As String = "RANDOM"
    Public Const LIFO As String = "LIFO"
    Public Const FIFO As String = "FIFO"
End Class

Public Class PICKTYPE
    Public Const FULLPICK As String = "FULLPICK"
    Public Const PARTIALPICK As String = "PARTIAL"
    Public Const NEGATIVEPALLETPICK As String = "NPP"
    Public Const PARALLELPICK As String = "PARPICK"
End Class

Public Class SCREENS
    Public Const LOGIN As String = "m4nScreens/Login.aspx"
    Public Const WAREHOUSELOGIN As String = "m4nScreens/LoginWarehouse.aspx"
    Public Const MAIN As String = "Screens/Main.aspx"
    Public Const USER_INFO As String = "Screens/UserInfo.aspx"
    Public Const DOC_VIEWER As String = "DocViewer.aspx"
    Public Const TASKMANAGER As String = "Screens/taskmanager.aspx"
    Public Const BATCHREPLENUNLOAD As String = "Screens/BATCHRPU.aspx"
    Public Const BATCHREPLENUNLOAD1 As String = "Screens/BATCHRPU1.aspx"
    Public Const BATCHREPLENLETDOWN As String = "Screens/BATCHRPL.aspx"
    Public Const BATCHREPLENLETDOWN1 As String = "Screens/BATCHRPL1.aspx"
    Public Const BATCHREPLENLPRINT As String = "Screens/BATCHREPLPRINT.aspx"
End Class

Public Class CAPTURETYPES
    Public Const INBOUND As String = "INBOUND"
    Public Const OUTBOUND As String = "OUTBOUND"
End Class

Public Class RECEIVINGWEIGHTCAPTUREMETHODS
    Public Const ACTUALBYLOAD As String = "LOAD"
    Public Const NOCAPTURE As String = "NONE"
    Public Const RECEIPTAVERAGE As String = "RCTAVG"
    Public Const ACTUALBYUOM As String = "UOM"
    Public Const ACTUALBYUOMBYID As String = "UOMID"
End Class

Public Class FULLPICKALLOCATION
    Public Const ALWAYS As String = "ALWAYS"
    Public Const BYSKU As String = "BYSKU"
End Class

Namespace PickMethods

    Public Class PickMethod
        Public Const DIRECTPICK As String = "DIRECT"
        Public Const PICKBYITEM As String = "PBI"
        Public Const PICKBYCUSTOMER As String = "PBC"
        Public Const PICKBYCOMPANYGROUP As String = "PBCG"
        Public Const PICKBYSHIPTO As String = "PBST"
        Public Const PICKBYROUTE As String = "PBR"
        Public Const PICKBYORDER As String = "PBO"
        Public Const PICKBYTRUCK As String = "PBT"
        'Public Const PARALELORDERPICKING As String = "POP"
        Public Const PICKBYWAVE As String = "PBW"
        Public Const DISCREETPICK As String = "DISC"
        'Public Const BATCHPICK As String = "BATCH"
    End Class

End Namespace

Namespace Actions

    Public Class Audit
        Public Shared ReadOnly Property ASNINS As String
            Get
                Return "ASNINS"
            End Get
        End Property
        Public Shared ReadOnly Property ASNUPD As String
            Get
                Return "ASNUPD"
            End Get
        End Property
        Public Shared ReadOnly Property COMPANYINS As String
            Get
                Return "COMPANYINS"
            End Get
        End Property
        Public Shared ReadOnly Property COMPANYUDP As String
            Get
                Return "COMPANYUDP"
            End Get
        End Property
        Public Shared ReadOnly Property CONSIGNEEINS As String
            Get
                Return "CONSIGNEEINS"
            End Get
        End Property
        Public Shared ReadOnly Property CONSIGNEEUPD As String
            Get
                Return "CONSIGNEEUPD"
            End Get
        End Property
        Public Shared ReadOnly Property CONTACTINS As String
            Get
                Return "CONTACTINS"
            End Get
        End Property
        Public Shared ReadOnly Property CONTACTUPD As String
            Get
                Return "CONTACTUPD"
            End Get
        End Property
        Public Shared ReadOnly Property INBOUNDLNINS As String
            Get
                Return "INBOUNDLNINS"
            End Get
        End Property
        Public Shared ReadOnly Property INBOUNDLNDEL As String
            Get
                Return "INBOUNDLNDEL"
            End Get
        End Property
        Public Shared ReadOnly Property INBOUNDLNUPD As String
            Get
                Return "INBOUNDLNUPD"
            End Get
        End Property
        Public Shared ReadOnly Property INBOUNDHINS As String
            Get
                Return "INBOUNDHINS"
            End Get
        End Property
        Public Shared ReadOnly Property INBOUNDHUPD As String
            Get
                Return "INBOUNDHUPD"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDLNINS As String
            Get
                Return "OUTBOUNDLNINS"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDLNDELETED As String
            Get
                Return "OUTBOUNDLNDEL"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDLNUPD As String
            Get
                Return "OUTBOUNDLNUPD"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDLNCNCLEXC As String
            Get
                Return "OUTBOUNDLNCNCLEXC"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHINS As String
            Get
                Return "OUTBOUNDHINS"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHUPD As String
            Get
                Return "OUTBOUNDHUPD"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHDEL As String
            Get
                Return "OUTBOUNDHDEL"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHPAK As String
            Get
                Return "OUTBOUNDHPAK"
            End Get
        End Property
        Public Shared ReadOnly Property FLOWTHPAK As String
            Get
                Return "FLOWTHPAK"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHPCK As String
            Get
                Return "OUTBOUNDHPCK"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHPLN As String
            Get
                Return "OUTBOUNDHPLN"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHSHP As String
            Get
                Return "OUTBOUNDHSHP"
            End Get
        End Property
        Public Shared ReadOnly Property FLOWTHSHP As String
            Get
                Return "FLOWTHSHP"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDCANC As String
            Get
                Return "OUTBOUNDCANC"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHLD As String
            Get
                Return "OUTBOUNDHLD"
            End Get
        End Property
        Public Shared ReadOnly Property FLOWTHLD As String
            Get
                Return "FLOWTHLD"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHSTG As String
            Get
                Return "OUTBOUNDHSTG"
            End Get
        End Property
        Public Shared ReadOnly Property OUTBOUNDHVRF As String
            Get
                Return "OUTBOUNDHVRF"
            End Get
        End Property
        Public Shared ReadOnly Property FLOWTHSTG As String
            Get
                Return "FLOWTHSTG"
            End Get
        End Property
        Public Shared ReadOnly Property LOCATIONINS As String
            Get
                Return "LOCATIONINS"
            End Get
        End Property
        Public Shared ReadOnly Property LOCATIONUPD As String
            Get
                Return "LOCATIONUPD"
            End Get
        End Property

        'Added for RWMS-2510 Start
        Public Shared ReadOnly Property LOCATIONDEL As String
            Get
                Return "LOCATIONDEL"
            End Get
        End Property
        Public Shared ReadOnly Property PICKLOCDEL As String
            Get
                Return "PICKLOCDEL"
            End Get
        End Property
        Public Shared ReadOnly Property PICKLOCINS As String
            Get
                Return "PICKLOCINS"
            End Get
        End Property
        Public Shared ReadOnly Property PICKLOCUDP As String
            Get
                Return "PICKLOCUDP"
            End Get
        End Property
        'Added for RWMS-2510 End

        Public Shared ReadOnly Property ORDSTUPD As String
            Get
                Return "ORDSTUPD"
            End Get
        End Property
        Public Shared ReadOnly Property PCKINS As String
            Get
                Return "PCKINS"
            End Get
        End Property
        Public Shared ReadOnly Property PCKUPD As String
            Get
                Return "PCKUPD"
            End Get
        End Property
        Public Shared ReadOnly Property PCKCOMPL As String
            Get
                Return "PCKCOMPL"
            End Get
        End Property
        'cad-picklist assign
        Public Shared ReadOnly Property PICKLISTASSIGN As String
            Get
                Return "PICKLISTASSIGN"
            End Get
        End Property

        Public Shared ReadOnly Property PCKLNINS As String
            Get
                Return "PCKLNINS"
            End Get
        End Property
        Public Shared ReadOnly Property PCKLNUPD As String
            Get
                Return "PCKLNUPD"
            End Get
        End Property

        Public Shared ReadOnly Property PLANWAVE As String
            Get
                Return "WAVEPLAN"
            End Get
        End Property
        Public Shared ReadOnly Property WAVEPLANED As String
            Get
                Return "WAVEPLANED"
            End Get
        End Property
        Public Shared ReadOnly Property COMPLETEWAVE As String
            Get
                Return "WAVECOMP"
            End Get
        End Property
        Public Shared ReadOnly Property CANWAVE As String
            Get
                Return "WAVECAN"
            End Get
        End Property
        Public Shared ReadOnly Property WAVEINS As String
            Get
                Return "WAVEINS"
            End Get
        End Property
        Public Shared ReadOnly Property WAVEUPD As String
            Get
                Return "WAVEUPD"
            End Get
        End Property
        Public Shared ReadOnly Property WAVEASGN As String
            Get
                Return "WAVEASGN"
            End Get
        End Property
        Public Shared ReadOnly Property WAVECANCEX As String
            Get
                Return "WAVECANCEX"
            End Get
        End Property

        Public Shared ReadOnly Property WOINS As String
            Get
                Return "WOINS"
            End Get
        End Property
        Public Shared ReadOnly Property WOUPD As String
            Get
                Return "WOUPD"
            End Get
        End Property
        Public Shared ReadOnly Property WOCNL As String
            Get
                Return "WOCNL"
            End Get
        End Property
        Public Shared ReadOnly Property WOEXEC As String
            Get
                Return "WOEXEC"
            End Get
        End Property

        Public Shared ReadOnly Property CREATELOAD As String
            Get
                Return "CREATELOAD"
            End Get
        End Property
        Public Shared ReadOnly Property UNRECEIVELOAD As String
            Get
                Return "UNRCVLOAD"
            End Get
        End Property
        Public Shared ReadOnly Property COUNTLOAD As String
            Get
                Return "COUNTLOAD"
            End Get
        End Property
        Public Shared ReadOnly Property CREATERECEIPT As String
            Get
                Return "CREATEREC"
            End Get
        End Property
        Public Shared ReadOnly Property CREATERECEIPTLINE As String
            Get
                Return "CREATERCLN"
            End Get
        End Property
        Public Shared ReadOnly Property CONSOLIDATE As String
            Get
                Return "CONSOLIDATE"
            End Get
        End Property
        Public Shared ReadOnly Property REQUESTPICKUP As String
            Get
                Return "RPK"
            End Get
        End Property
        Public Shared ReadOnly Property OVERRIDEPW As String
            Get
                Return "OVERRIDEPW"
            End Get
        End Property
        Public Shared ReadOnly Property LOADPUTAWAY As String
            Get
                Return "LDPUTAWAY"
            End Get
        End Property
        Public Shared ReadOnly Property CONTAINERPUTAWAY As String
            Get
                Return "CNTPUTAWAY"
            End Get
        End Property
        Public Shared ReadOnly Property RELEASEWAVE As String
            Get
                Return "WAVEREL"
            End Get
        End Property
        Public Shared ReadOnly Property RELEASEPICKLIST As String
            Get
                Return "PCKLREL"
            End Get
        End Property
        Public Shared ReadOnly Property LOADREPL As String
            Get
                Return "LOADREPL"
            End Get
        End Property

        Public Shared ReadOnly Property PLANROUTINGSET As String
            Get
                Return "RTSETPLAN"
            End Get
        End Property
        Public Shared ReadOnly Property PLANWITHRUNIDROUTINGSET As String
            Get
                Return "RTSETPLANWITHRUNID"
            End Get
        End Property
        Public Shared ReadOnly Property REPLANROUTINGSET As String
            Get
                Return "RTSETREPLAN"
            End Get
        End Property
        Public Shared ReadOnly Property BACKHAULROUTINGSET As String
            Get
                Return "RTSETBACKHAUL"
            End Get
        End Property


        Public Shared ReadOnly Property PLANROUTE As String
            Get
                Return "RTPLAN"
            End Get
        End Property
        Public Shared ReadOnly Property PLANORDER As String
            Get
                Return "ORDERPLAN"
            End Get
        End Property


        Public Shared ReadOnly Property TASKINS As String
            Get
                Return "TASKINS"
            End Get
        End Property
        Public Shared ReadOnly Property TASKUPD As String
            Get
                Return "TASKUPD"
            End Get
        End Property
        Public Shared ReadOnly Property TASKCNL As String
            Get
                Return "TASKCNL"
            End Get
        End Property
        Public Shared ReadOnly Property TASKASGN As String
            Get
                Return "TASKASGN"
            End Get
        End Property
        Public Shared ReadOnly Property TASKREL As String
            Get
                Return "TASKREL"
            End Get
        End Property
        Public Shared ReadOnly Property TASKCOMP As String
            Get
                Return "TASKCOMP"
            End Get
        End Property

        Public Shared ReadOnly Property COMPLETESHIP As String
            Get
                Return "SHIPCOMP"
            End Get
        End Property
        Public Shared ReadOnly Property COMPLETEORDER As String
            Get
                Return "ORDERCOMP"
            End Get
        End Property

        Public Shared ReadOnly Property RECEIPTCLOSE As String
            Get
                Return "RECCLOSE"
            End Get
        End Property
        Public Shared ReadOnly Property RECEIPTLNINS As String
            Get
                Return "RECEIPTLNINS"
            End Get
        End Property
        Public Shared ReadOnly Property RECEIPTLNUPD As String
            Get
                Return "RECEIPTLNUPD"
            End Get
        End Property
        Public Shared ReadOnly Property RECEIPTLNDEL As String
            Get
                Return "RECEIPTLNDEL"
            End Get
        End Property
        Public Shared ReadOnly Property RECEIPTHINS As String
            Get
                Return "RECEIPTHINS"
            End Get
        End Property
        Public Shared ReadOnly Property RECEIPTHUPD As String
            Get
                Return "RECEIPTHUPD"
            End Get
        End Property
        Public Shared ReadOnly Property RECEIPTHCNCL As String
            Get
                Return "RECEIPTHCNCL"
            End Get
        End Property
        Public Shared ReadOnly Property RECEIPTHADC As String
            Get
                Return "RECEIPTHADC"
            End Get
        End Property

        Public Shared ReadOnly Property REPLCOMPL As String
            Get
                Return "REPLCOMPL"
            End Get
        End Property
        Public Shared ReadOnly Property REPLENISHMENT As String
            Get
                Return "REPLENISHMENT"
            End Get
        End Property

        Public Shared ReadOnly Property SHIPMENTINS As String
            Get
                Return "SHIPMENTINS"
            End Get
        End Property
        Public Shared ReadOnly Property SHIPMENTUPD As String
            Get
                Return "SHIPMENTUPD"
            End Get
        End Property
        Public Shared ReadOnly Property SHIPSTTCHNG As String
            Get
                Return "SHIPSTTCHNG"
            End Get
        End Property
        Public Shared ReadOnly Property SHIPATDOCK As String
            Get
                Return "SHIPATDOCK"
            End Get
        End Property
        Public Shared ReadOnly Property ORDPLANED As String
            Get
                Return "ORDPLANED"
            End Get
        End Property
        Public Shared ReadOnly Property SHIPMENTLOADING As String
            Get
                Return "SHIPLOADING"
            End Get
        End Property

        Public Shared ReadOnly Property PICKLOAD As String
            Get
                Return "PICKLOAD"
            End Get
        End Property
        Public Shared ReadOnly Property PACKLOAD As String
            Get
                Return "PACKLOAD"
            End Get
        End Property
        Public Shared ReadOnly Property UNPACKLOAD As String
            Get
                Return "UNPACKLOAD"
            End Get
        End Property
        Public Shared ReadOnly Property LOADLOAD As String
            Get
                Return "LOADLOAD"
            End Get
        End Property
        Public Shared ReadOnly Property UNLOADLOAD As String
            Get
                Return "UNLOADLOAD"
            End Get
        End Property

        Public Shared ReadOnly Property LOADVER As String
            Get
                Return "LOADVER"
            End Get
        End Property
        Public Shared ReadOnly Property LOADMERGE As String
            Get
                Return "LOADMERGE"
            End Get
        End Property
        Public Shared ReadOnly Property UNPICK As String
            Get
                Return "UNPICK"
            End Get
        End Property
        Public Shared ReadOnly Property SHIPLOAD As String
            Get
                Return "SHIPLOAD"
            End Get
        End Property
        Public Shared ReadOnly Property SETUOM As String
            Get
                Return "SETUOM"
            End Get
        End Property
        Public Shared ReadOnly Property ORDERSHIP As String
            Get
                Return "ORDERSHIP"
            End Get
        End Property
        Public Shared ReadOnly Property SHPSHIP As String
            Get
                Return "SHPSHIP"
            End Get
        End Property
        Public Shared ReadOnly Property CANSHIP As String
            Get
                Return "SHPCAN"
            End Get
        End Property
        Public Shared ReadOnly Property SNAPSHOT As String
            Get
                Return "SNAPSHOT"
            End Get
        End Property
        Public Shared ReadOnly Property CANPCKLIST As String
            Get
                Return "CANPCKLIST"
            End Get
        End Property
        Public Shared ReadOnly Property CANCELPICK As String
            Get
                Return "CANPICK"
            End Get
        End Property
        Public Shared ReadOnly Property UNALLOCATEPICK As String
            Get
                Return "UNALLOC"
            End Get
        End Property
        Public Shared ReadOnly Property UNALLOCATEPICKLIST As String
            Get
                Return "UNALLLIST"
            End Get
        End Property
        Public Shared ReadOnly Property PCKPCKLIST As String
            Get
                Return "PCKPCKLIST"
            End Get
        End Property
        Public Shared ReadOnly Property ADDQTY As String
            Get
                Return "ADDQTY"
            End Get
        End Property
        Public Shared ReadOnly Property SUBQTY As String
            Get
                Return "SUBQTY"
            End Get
        End Property
        'RWMS-1483 and RWMS-1250
        Public Shared ReadOnly Property NEWQTY As String
            Get
                Return "NEWQTY"
            End Get
        End Property
        'End RWMS-1483 and RWMS-1250
        Public Shared ReadOnly Property SETSTATUS As String
            Get
                Return "SETSTATUS"
            End Get
        End Property
        Public Shared ReadOnly Property SETORDSTAT As String
            Get
                Return "SETORDSTAT"
            End Get
        End Property
        Public Shared ReadOnly Property MOVELOAD As String
            Get
                Return "MOVELOAD"
            End Get
        End Property
        Public Shared ReadOnly Property LOADSTATCHANGED As String
            Get
                Return "LDSTATCHNG"
            End Get
        End Property
        Public Shared ReadOnly Property DELETELOAD As String
            Get
                Return "DELLD"
            End Get
        End Property
        Public Shared ReadOnly Property SPLITLOAD As String
            Get
                Return "SPLITLD"
            End Get
        End Property
        Public Shared ReadOnly Property SPLITREPL As String
            Get
                Return "SPLITREPL"
            End Get
        End Property
        Public Shared ReadOnly Property SPLITCONTAINER As String
            Get
                Return "SPLITCONT"
            End Get
        End Property
        Public Shared ReadOnly Property SPLITPICK As String
            Get
                Return "SPLITPICK"
            End Get
        End Property
        Public Shared ReadOnly Property CHANGEUOM As String
            Get
                Return "CHANGEUOM"
            End Get
        End Property
        Public Shared ReadOnly Property LOCATIONCOUNT As String
            Get
                Return "LOCCOUNT"
            End Get
        End Property
        Public Shared ReadOnly Property LOCATIONPRBLM As String
            Get
                Return "LOCATIONPRBLM"
            End Get
        End Property
        'Addde for RWMS-1554 and RWMS-1506 Start
        Public Shared ReadOnly Property CANCELLOCATIONPRBLM As String
            Get
                Return "CANCELLOCATIONPRBLM"
            End Get
        End Property
        'Addde for RWMS-1554 and RWMS-1506 End

        Public Shared ReadOnly Property PUTAWAYOVERRIED As String
            Get
                Return "PWTOVRD"
            End Get
        End Property

        Public Shared ReadOnly Property ORDASGNWAV As String
            Get
                Return "ORDASGNWAV"
            End Get
        End Property
        Public Shared ReadOnly Property DASGNORWAV As String
            Get
                Return "DASGNORWAV"
            End Get
        End Property
        Public Shared ReadOnly Property STAGELOAD As String
            Get
                Return "STAGELOAD"
            End Get
        End Property
        Public Shared ReadOnly Property FLWTHLNDEL As String
            Get
                Return "FLWTHLNDEL"
            End Get
        End Property
        'Added for PWMS-756
        Public Shared ReadOnly Property RECOVRRD As String
            Get
                Return "RECOVRRD"
            End Get
        End Property
        Public Shared ReadOnly Property WGTOVRRD As String
            Get
                Return "WGTOVRRD"
            End Get
        End Property
        Public Shared ReadOnly Property LOCASGNPRB As String
            Get
                Return "LOCASGNPRB"
            End Get
        End Property

        'Added for PWMS-810 and RWMS-791
        Public Shared ReadOnly Property SHIPMENTMOVED As String
            Get
                Return "SHIPMENTMOVED"
            End Get
        End Property
        Public Shared ReadOnly Property ORDERLINEMOVED As String
            Get
                Return "ORDERLINEMOVED"
            End Get
        End Property
        'End Added for PWMS-810 and RWMS-791
        Public Shared ReadOnly Property REVIEW_REPLEN As String
            Get
                Return "REVIEW_REPLEN"
            End Get
        End Property

        'Added for RWMS-2540 Start
        Public Shared ReadOnly Property LOGIN As String
            Get
                Return "LOGIN"
            End Get
        End Property
        Public Shared ReadOnly Property LOGOUT As String
            Get
                Return "LOGOUT"
            End Get
        End Property
        'Added for RWMS-2540 End
        Public Shared ReadOnly Property BATCHREPLSCHEDULED As String
            Get
                Return "BATCHREPLSCHEDULED"
            End Get
        End Property
        Public Shared ReadOnly Property BATCHREPLLETDOWN As String
            Get
                Return "BATCHREPLLETDOWN"
            End Get
        End Property
        Public Shared ReadOnly Property BATCHREPLUNLOAD As String
            Get
                Return "BATCHREPLUNLOAD"
            End Get
        End Property

        Public Shared ReadOnly Property BATCHREPLPLANNED As String
            Get
                Return "BATCHREPLPLANNED"
            End Get
        End Property
        Public Shared ReadOnly Property BATCHREPLRELEASED As String
            Get
                Return "BATCHREPLRELEASED"
            End Get
        End Property
        Public Shared ReadOnly Property BATCHREPLCOMPLETED As String
            Get
                Return "BATCHREPLCOMPLETED"
            End Get
        End Property
    End Class

    Public Class Services
        Public Const OVERRIDEPW As String = "OVERRIDEPW"
        Public Const PUTAWAY As String = "PUTAWAY"
        Public Const REQUESTLOCATIONFORCONTAINER As String = "RQLOCCNT"
        Public Const REQUESTLOCATIONFORLOAD As String = "RQLOCLD"
        Public Const REQUESTLOCATIONBYLOC As String = "RQLOCBYLOC"
        Public Const REQUESTLOCATIONBYREGION As String = "RQLOCBYREG"
        Public Const REQUESTLOCATIONFORMULTILOAD As String = "RQLOCLDMULTI"
    End Class

End Namespace

Namespace Plan

    Public Class SUBSTITUTESKUMODE
        Public Const NEVER As String = "NEVER"
        Public Const ONSHORT As String = "ONSHORT"
    End Class

    Public Class PLANMODES
        Public Const NONE As String = "NONE"
        Public Const PLAN As String = "PLAN"
        Public Const PLANANDRELEASE As String = "PLNRLS"
    End Class

End Namespace

Namespace Release

    Public Class AUTOPRINTPICKLABEL
        Public Const ONRELEASE As String = "ONRELEASE"
        Public Const ONPCKSTART As String = "ONPCKSTART"
        Public Const ONPCKDETAILCOMPLETE As String = "ONPCKCOMP"
    End Class
    Public Class BAGOUTPRINTOPTION
        Public Const ONPCKSTART As String = "ONPCKSTART"
        Public Const ONPCKCOMPLETE As String = "ONPCKCOMP"
    End Class
    Public Class AUTORELEASE
        Public Const NO As String = "NO"
        Public Const PLANANDRELEASE As String = "PLNRLS"
    End Class

    Public Class PICKLABELTYPE
        Public Const NONE As String = "NONE"
        Public Const PERPICK As String = "PERPCK"
        Public Const PERUOM As String = "PERUOM"
        Public Const PICKTITLE As String = "PCKTITLE"
        Public Const PERPICKANDTITLE As String = "PERPCKTITL"
        Public Const PERUOMANDTITLE As String = "PERUOMTITL"
    End Class

    Public Class SHIPLABELTYPE
        Public Const NONE As String = "NONE"
        Public Const ONSTART As String = "ONSTART"
        Public Const ONEND As String = "ONEND"
    End Class

    Public Class CONFIRMATIONTYPE
        Public Const NONE As String = "NONE"
        Public Const LOCATION As String = "LOCATION"
        Public Const LOAD As String = "LOAD"
        Public Const UPC As String = "UPC"
        Public Const SKU As String = "SKU"
        Public Const SKULOCATION As String = "SKULOC"
        Public Const SKULOCATIONUOM As String = "SKULOCUOM"
        Public Const SKUUOM As String = "SKUUOM"
    End Class

    Public Class LOADINGCONFIRMATIONTYPE
        Public Const NONE As String = "NONE"
        Public Const VEHICLELOCATION As String = "VEHLOC"
        Public Const VEHICLE As String = "VEHICLE"
        Public Const DOCK As String = "DOCK"
    End Class

End Namespace

Namespace Statuses

    Public Class TransShipment
        Public Const STATUSNEW As String = "NEW"
        Public Const CANCELED As String = "CANCELED"
        Public Const RECEIVED As String = "RECEIVED"
        Public Const SHIPPED As String = "SHIPPED"
    End Class

    Public Class Flowthrough
        Public Const STATUSNEW As String = "NEW"
        Public Const CANCELED As String = "CANCELED"
        Public Const RECEIVING As String = "RECEIVING"
        Public Const RECEIVED As String = "RECEIVED"
        Public Const LOADING As String = "LOADING"
        Public Const LOADED As String = "LOADED"
        Public Const SHIPPING As String = "SHIPPING"
        Public Const SHIPPED As String = "SHIPPED"
        Public Const VERIFIED As String = "VERIFIED"
    End Class

    Public Class Receipt
        Public Const STATUSNEW As String = "NEW"
        Public Const RECEIVING As String = "RECEIVING"
        Public Const SCHEDULED As String = "SCHEDULED"
        Public Const ATDOCK As String = "ATDOCK"
        Public Const CANCELLED As String = "CANCELLED"
        Public Const CLOSED As String = "CLOSE"
    End Class

    Public Class ASN
        Public Const EXPECTED As String = "EXPECTED"
        Public Const RECEIVED As String = "RECEIVED"
    End Class

    Public Class Replenishment
        Public Const PLANNED As String = "PLANNED"
        Public Const COMPLETE As String = "COMPLETE"
        Public Const CANCELED As String = "CANCELED"
    End Class
    Public Class BatchReplensishment
        Public Const PLANNED As String = "PLANNED"
        Public Const RELEASED As String = "RELEASED"
        Public Const CANCELED As String = "CANCELED"
        Public Const COMPLETE As String = "COMPLETE"
        Public Const LETDOWN As String = "LETDOWN"
        Public Const UNLOAD As String = "UNLOAD"

    End Class
    Public Class Task
        Public Const AVAILABLE As String = "AVAILABLE"
        Public Const COMPLETE As String = "COMPLETE"
        Public Const ASSIGNED As String = "ASSIGNED"
        Public Const CANCELED As String = "CANCELED"
    End Class

    Public Class InventoryAdjustmentLine
        Public Const STATUSNEW As String = "NEW"
        Public Const CANCELED As String = "CANCELED"
        Public Const POSTED As String = "POSTED"
    End Class

    Public Class InboundOrderHeader
        Public Const STATUSNEW As String = "NEW"
        Public Const RECEIVING As String = "RECEIVING"
        Public Const CLOSED As String = "CLOSE"
        Public Const CANCELED As String = "CANCELED"
    End Class

    Public Class ActivityStatus
        Public Const NONE As String = ""
        Public Const PACKED As String = "PACKED"
        Public Const PICKED As String = "PICKED"
        Public Const LOADED As String = "LOADED"
        Public Const STAGED As String = "STAGED"
        Public Const PICKLOAD As String = "PICKLOAD"
        Public Const LOCASSIGNPEND As String = "LOCPEND"
        Public Const PUTAWAYPEND As String = "PWPEND"
        Public Const PICKPEND As String = "PICKPEND"
        Public Const VERIFIED As String = "VERIFIED"
        Public Const REPLPENDING As String = "REPLPEND"
        Public Const ALLOCPENDING As String = "ALLOCPEND"
    End Class

    Public Class LoadStatus
        Public Const NONE As String = ""
        Public Const LIMBO As String = "LIMBO"
        Public Const AVAILABLE As String = "AVAILABLE"
    End Class

    Public Class OutboundOrderHeader
        Public Const STATUSNEW As String = "NEW"
        Public Const SHIPMENTASSIGNED As String = "SASSIGNED"
        Public Const ROUTINGSETASSIGNED As String = "RASSIGNED"
        Public Const WAVEASSIGNED As String = "WASSIGNED"
        Public Const RELEASED As String = "RELEASED"
        Public Const RELEASING As String = "RELEASING"
        Public Const PLANNED As String = "PLANNED"
        Public Const PLANNING As String = "PLANNING"
        Public Const PICKING As String = "PICKING"
        Public Const PICKED As String = "PICKED"
        Public Const STAGED As String = "STAGED"
        Public Const PACKED As String = "PACKED"
        Public Const LOADING As String = "LOADING"
        Public Const LOADED As String = "LOADED"
        Public Const SHIPPING As String = "SHIPPING"
        Public Const SHIPPED As String = "SHIPPED"
        Public Const CANCELED As String = "CANCELED"
        Public Const VERIFIED As String = "VERIFIED"
    End Class

    Public Class BatchReplenHeader
        Public Const RELEASED As String = "RELEASED"
        Public Const LETDOWN As String = "LETDOWN"
        Public Const UNLOAD As String = "UNLOAD"
    End Class

    Public Class BatchReplenDetail
        Public Const RELEASED As String = "RELEASED"
        Public Const LETDOWN As String = "LETDOWN"
        Public Const UNLOAD As String = "UNLOAD"
    End Class

    Public Class Shipment
        Public Const STATUSNEW As String = "NEW"
        Public Const ASSIGNED As String = "ASSIGNED"
        Public Const LOADING As String = "LOADING"
        Public Const SHEDULED As String = "SHEDULED"
        Public Const ATDOCK As String = "ATDOCK"
        Public Const LOADED As String = "LOADED"
        Public Const SHIPPING As String = "SHIPPING"
        Public Const SHIPPED As String = "SHIPPED"
        Public Const CANCELED As String = "CANCELED"
    End Class

    Public Class Wave
        Public Const STATUSNEW As String = "NEW"
        Public Const ASSIGNED As String = "ASSIGNED"
        Public Const PLANNING As String = "PLANNING"
        Public Const PLANNED As String = "PLANNED"
        Public Const RELEASING As String = "RELEASING"
        Public Const RELEASED As String = "RELEASED"
        Public Const COMPLETING As String = "COMPLETING"
        Public Const COMPLETE As String = "COMPLETE"
        Public Const CANCELED As String = "CANCELED"
    End Class

    Public Class RoutingSet
        Public Const STATUSNEW As String = "NEW"
        Public Const ASSIGNED As String = "ASSIGNED"
        Public Const PLANNING As String = "PLANNING"
        Public Const PLANNED As String = "PLANNED"
        Public Const CANCELED As String = "CANCELED"
    End Class

    Public Class Picklist
        Public Const PLANNED As String = "PLANNED"
        Public Const RELEASING As String = "RELEASING"
        Public Const RELEASED As String = "RELEASED"
        Public Const CANCELED As String = "CANCELED"
        Public Const COMPLETE As String = "COMPLETE"
        Public Const PARTPICKED As String = "PARTPICKED"
    End Class

    Public Class Consolidation
        Public Const AVAILABLE As String = "AVAILABLE"
        Public Const ASSIGNED As String = "ASSIGNED"
        Public Const CANCELED As String = "CANCELED"
        Public Const COMPLETE As String = "COMPLETE"
    End Class

    Public Class ParallelPickList
        Public Const AVAILABLE As String = "AVAILABLE"
        Public Const ASSIGNED As String = "ASSIGNED"
        Public Const CANCELED As String = "CANCELED"
        Public Const COMPLETE As String = "COMPLETE"
    End Class

    Public Class Container
        Public Const STATUSNEW As String = "NEW"
        'RWMS-2497 - Replaced DELIVERED with MANUALPUT
        Public Const DELIVERED As String = "MANUALPUT"
        Public Const DELIVERYPEND As String = "DELPEND"
        Public Const PACKED As String = "PACKED"
        Public Const STAGED As String = "STAGED"
        Public Const LOADED As String = "LOADED"
        Public Const SHIPPED As String = "SHIPPED"
    End Class

    Public Class YardEntry
        Public Const STATUSNEW As String = "NEW"
        Public Const SCHEDULED As String = "SCHEDULED"
        Public Const INYARD As String = "INYARD"
        Public Const DEPARTED As String = "DEPARTED"
        Public Const CANCELED As String = "CANCELED"
    End Class

    Public Class CountBook
        Public Const [NEW] As String = "NEW"
        Public Const COMPLETE As String = "COMPLETE"
        Public Const COUNTING As String = "COUNTING"
        Public Const CANCELLED As String = "CANCELLED"
    End Class

    Public Class Counting
        Public Const PLANNED As String = "PLANNED"
        Public Const COMPLETE As String = "COMPLETE"
        Public Const CANCELED As String = "CANCELED"
    End Class

    Public Class RoutePackages
        Public Const [NEW] As String = "NEW"
        Public Const LOADED As String = "LOADED"
        Public Const DELIVERED As String = "DELIVERED"
        Public Const DELIVEREDPARTIAL As String = "PARTDELIVERED"
        Public Const PICKEDUP As String = "PICKEDUP"
        Public Const OFFLOADED As String = "OFFLOADED"
    End Class

    Public Class RouteTasks
        Public Const [NEW] As String = "NEW"
        Public Const COMPLETED As String = "COMPLETED"
        Public Const INCOMPLETE As String = "INCOMPLETE"
        Public Const CANCELLED As String = "CANCELLED"
    End Class

    Public Class PackingLists
        Public Const [NEW] As String = "NEW"
        Public Const ASSIGNED As String = "ASSIGNED"
        Public Const SHIPPED As String = "SHIPPED"
        Public Const CANCELLED As String = "CANCELLED"
    End Class

End Namespace

Namespace LaborPerFormance

    Public Class LaborCounterScope
        Public Const Permanent As String = "PERMANENT"
        Public Const ClockIn As String = "CLOCKIN"
        Public Const ShiftStart As String = "SHIFTSTART"
        Public Const Task As String = "TASK"
    End Class

    Public Enum LaborPerfDistances
        FromLocWalkDistance = 2
        FromLocAisleDistance = 1
        OutOfAisleDistance = 0
        ToLocAisleDistance = 3
        ToLocWalkDistance = 4
    End Enum

    Public Class LaborPerfDistancesNames
        'OutOfAisleDistance  0
        'FromLocWalkDistance 1
        'FromLocAisleDistance 2
        'ToLocWalkDistance 3
        'ToLocAisleDistance 4
        Public Shared DistanceArray() As String = {
        "OutOfAisleDistance",
        "FromLocWalkDistance",
        "FromLocAisleDistance",
        "ToLocAisleDistance",
        "ToLocWalkDistance"
        }
    End Class

    Public Class DistanceType
        Public Const Assign As String = "Assign"
        Public Const Tasks As String = "Task"
    End Class

End Namespace

Namespace Shift
    Public Class ShiftInstanceStatus
        Public Const [NEW] As String = "NEW"
        Public Const CLOSED As String = "CLOSED"
        Public Const STARTED As String = "STARTED"
        Public Const CANCELED As String = "CANCELED"
    End Class

    Public Class ClockStatus
        Public Const [IN] As Integer = 1
        Public Const [OUT] As Integer = 2
    End Class

End Namespace