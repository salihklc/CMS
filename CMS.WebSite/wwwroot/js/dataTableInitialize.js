"use strict";
var globalDTobj;
// $.extend(true, $.fn.dataTable.defaults, {

// });


// function onRender(data, type, row, meta) {
//     // <a class="btn btn-default btn-md" asp-route-Idx="@user.Idx" asp-action="Edit" asp-controller="User" asp-area="Admin">
//     //                     <i class="fa fa-edit"></i> </a>
//     //                     <a class="btn btn-default btn-md remove-btn-ajax" asp-action="Delete" asp-controller="User" asp-area="Admin" asp-route-Idx="@user.Idx">
//     //                     <i class="fa fa-remove"></i> </a>
//     return '<button type="button" data-type="view" class="btn btn-sm btn-default"><i class="fa fa-lg fa-fw fa-search"></i></button> <button type="button" data-type="remove" class="btn btn-sm btn-danger"><i class="fa fa-lg fa-fw fa-trash"></i></button>';
// }

// function onClick(e) {
//     if (e.data.type == 'remove') {
//         console.log('remove clicked');
//     } else if (e.data.type == 'view') {
//         console.log('view clicked');
//     }
// }
function occurrences(string, substring) {
    var n = 0;
    var pos = 0;
    if (string != undefined && string != null) {
        while (true) {
            pos = string.indexOf(substring, pos);
            if (pos != -1) { n++; pos += substring.length; }
            else { break; }
        }
    }

    return (n);
}
$(function () {

    var $thisDataTable = $('.default-datatable');
    var serverUrl = $thisDataTable.attr('data-server-uri');

    var ticketdatatable = $thisDataTable.attr("data-tickettable");

    var datatableOption = {};
    //2si uyumlu değil make your choose
    datatableOption.scrollX = true;
    //datatableOption.dom = "Rlfrtip";

    datatableOption.paging = true;
    datatableOption.lengthChange = true;
    datatableOption.searching = true;
    datatableOption.ordering = true;
    datatableOption.info = true;
    datatableOption.autoWidth = false;
    datatableOption.language = {
        "url": "/lib/datatables.net/i18n/turkish.json"
    }
    datatableOption.lengthMenu = [];
    datatableOption.lengthMenu.push([10, 25, 50, -1])
    datatableOption.lengthMenu.push(['10', '25', '50', 'All']);



    // datatableOption.searchDelay = 500;
    
    datatableOption.serverSide = true;
    datatableOption.processing = true;
    datatableOption["ajax"] = {
        "url": serverUrl,
        "type": "post"
    }

    

    datatableOption.columns = [];

    var allColumns = $thisDataTable.find("thead tr th"); //.attr("data-column");
    var defaultOrderArr = [];

    allColumns.each((k, v) => {
        let $v = $(v);
        let dataColumn = $v.attr("data-column") //.attributes["data-column"].value;
        let option = $v.attr("data-option") //v.attributes["data-option"].value;
        let columnIsSearchable = $v.attr("data-search");
        let columnIsOrderable = $v.attr("data-orderable");
        let defaultOrder = $v.attr("data-default-order");
        let columnWidth = $v.attr("data-column-width");

        let individualSearch = $v.attr("data-individual-search");

        if (individualSearch == "true") {

            var footerElem = $v.closest("table").find("tfoot th")[k];
            var innertText = footerElem.textContent;

            $(footerElem).html('<input class="grid-search-input" type="text" placeholder=" ' + innertText + '" />');

        }


        if (defaultOrder == "asc" || defaultOrder == "desc") {
            defaultOrderArr.push([k, defaultOrder]);
        }

        var dataTblObj = "";

        var isCrud = 0;

        if (option && (option.indexOf('U') != -1 || option.indexOf('D') != -1 || option.indexOf('C') != -1)) {

            let editUrl = $v.attr("data-edit-url");
            let deleteUrl = $v.attr("data-delete-url");
            let customContent = $v.attr("data-custom");
            let custombuttons;
            dataTblObj = {
                data: dataColumn, render: function (data, type, row) {
                    var obj = "";

                    if (option.indexOf('U') != -1)
                        obj += '<a class="btn btn-default btn-md" data-toggle="tooltip" title="DÃ¼zenle"' +
                            'href="' + editUrl + '?' + dataColumn + "=" + row[dataColumn] + '"><i class="fa fa-edit"></i> </a>';

                    if (option.indexOf('D') != -1)
                        obj += '<a class="btn btn-default btn-md remove-btn-ajax"  data-toggle="tooltip" title="Sil"'
                            + 'href="' + deleteUrl + '?' + dataColumn + "=" + row[dataColumn] + '"><i class="fas fa-trash-alt"></i> </a>';

                    if (option.indexOf('C') != -1) {

                        custombuttons = customContent;
                        var count = occurrences(customContent, "{0}");
                        for (var i = 0; i < count; i++) {
                            custombuttons = custombuttons.replace("{0}", row[dataColumn]);
                        }
                        obj += custombuttons

                    }


                    return obj;
                }, orderable: false, searchable: false
            }

            isCrud = 1;
        }

        if (isCrud != 1) {
            dataTblObj = {
                "data": dataColumn,
                "name": dataColumn,
                orderable: columnIsOrderable,
                searchable: columnIsSearchable
            };

            if (columnWidth != null && columnWidth.length > 0) {
                debugger;
                dataTblObj.width = parseInt(columnWidth);
            } else {
                dataTblObj.autoWidth = true;
            }
        }

        datatableOption.order = defaultOrderArr;

        datatableOption.columns.push(dataTblObj);
        if (ticketdatatable) {
            datatableOption.rowCallback = function (row, data, index) {
                console.log(data);
                if (data.TimeAfterCreation < 30) {
                    $('td', row).css('background-color', '#00a65a');
                }
                else if (data.TimeAfterCreation > 30 && data.TimeAfterCreation < 60) {
                    $('td', row).css('background-color', 'Orange');
                } else if (data.TimeAfterCreation > 60) {
                    $('td', row).css('background-color', '#ec8582');
                }
            }
        }

        datatableOption.initComplete = function (settings, json) {
            $('[data-toggle="tooltip"]').tooltip();

            //this.fnAdjustColumnSizing(true);

            renderedDT.columns().every(function () {
                var dTable = this;
                $(this.footer()).find("input").on("keyup change", function (e) {

                    if (e.keyCode == 13 || this.value == "") {
                        dTable.search(this.value).draw();
                    }

                });
            });


            $('.dataTables_filter input').unbind();


            $("div.dataTables_filter input").keyup(function (e) {
                if (e.keyCode == 13 || this.value == "") {
                    renderedDT.search(this.value).draw();
                }
            });

        }
    })

    globalDTobj = datatableOption;

    console.log(datatableOption);

    var renderedDT = $thisDataTable.DataTable(datatableOption);

    //try {
    //    //column resize plugin varsa 
    //    renderedDT = $thisDataTable.DataTable();
    //    new $.fn.dataTable.ColReorder(renderedDT);
    //} catch (e) {
    //    renderedDT = $thisDataTable.DataTable(datatableOption)
    //    console.log(e + "sayfaya resize plug-in yükle yok 💥.")
    //}
    


    // renderedDT.columns().every(function () {
    //     var that = this;

    //     $('input', this.footer()).on('keyup change clear', function () {
    //         debugger;
    //         if (that.search() !== this.value) {
    //             that
    //                 .search(this.value)
    //                 .draw();
    //         }
    //     });
    // });

    // renderedDT.columns().every( function () {
    //     debugger
    //     var that = this;    
    //     $( 'input', this.header() ).on( 'keydown', function (ev) {
    //          if (ev.keyCode == 13) { //only on enter keypress (code 13)
    //             that
    //                 .search( this.value )
    //                 .draw();
    //          }
    //     } );
    // } );

    setTimeout(function () {


    }, 100);

});
