var idTmr = ""; 


function toExcel(tableid) //读取表格中每个单元到EXCEL中 
{ 
    var curTbl = document.getElementById(tableid); 
     var oXL = new ActiveXObject("Excel.Application"); 
     //创建AX对象excel 
     var oWB = oXL.Workbooks.Add(); 
     //获取workbook对象 
    var oSheet = oWB.ActiveSheet; 
    //激活当前sheet 
     var Lenr = curTbl.rows.length; 
     //取得表格行数 
     for (i = 0; i < Lenr; i++) 
     { 
         var Lenc = curTbl.rows(i).cells.length; 
         //取得每行的列数 
         for (j = 0; j < Lenc; j++) 
         { 
             oSheet.Cells(i + 1, j + 1).value = curTbl.rows(i).cells(j).innerText; 
             //赋值 
         } 
     } 
     oXL.Visible = true; 
     //设置excel可见属性 
}


function copy(tabid) 
{ 
var oControlRange = document.body.createControlRange(); 
oControlRange.add(tabid,0); 
oControlRange.select(); 
document.execCommand("Copy"); 
} 
function toExcel(tabid){ 
copy(tabid); 
try 
{ 
var xls = new ActiveXObject( "Excel.Application" ); 
} 
catch(e) 
{ 
alert( "Excel没有安装或浏览器设置不正确.请启用所有Active控件和插件"); 
return false; 
} 
xls.visible = true; 
var xlBook = xls.Workbooks.Add; 
var xlsheet = xlBook.Worksheets(1); 
xlBook.Worksheets(1).Activate; 
for(var i=0;i<tabid.rows(0).cells.length;i++){ 
xlsheet.Columns(i+1).ColumnWidth=15; 
} 
xlsheet.Paste; 
xls=null; 
idTmr = window.setInterval("Cleanup();",1); 
} 
function Cleanup() { 
window.clearInterval(idTmr); 
CollectGarbage(); 
} 