// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
"use strict"
// Write your JavaScript code.
//var activeMenuSelecter = "";
$('.sidebar-menu').tree();

//Toast init
const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
})

if (activeMenuSelecter != "") {
    let selectedMenulink = $(".sidebar-menu " + activeMenuSelecter);

    selectedMenulink.closest("li").addClass("active");
    findClosestAndAddClassUntil(selectedMenulink, "li.treeview", "menu-open active", ".sidebar-menu");
}

function findClosestAndAddClassUntil(element, closestSelector, classString, untilClass) {
    let elementToActivate = $(element).parents(closestSelector);

    if (untilClass != undefined && untilClass != "") {
        if ($(elementToActivate).parents(untilClass).length == 0) {
            return;
        }
    }

    elementToActivate.addClass(classString);

    findClosestAndAddClassUntil(elementToActivate, closestSelector, classString, untilClass);

}



var Url = baseAdminPageUrl + "/Home/DashboardData"

//Tüm dropdownlar kendi ajax'ına baksın.
$.ajax({
    type: "post",
    url: Url,
    success: function (resp) {

    }
});


try {
    $.blockUI.defaults.overlayCSS.backgroundColor = '#000';
    $.blockUI.defaults.css.border = '';
    $.blockUI.defaults.css.backgroundColor = '';
    $.blockUI.defaults.css.color = '#fff';
    $.blockUI.defaults.message = '<h2><div><i class="fa fa-gear fa-spin"></i></div> İşleminiz gerçekleştiriliyor...';

    $(document).ajaxSend(function (event, jqxhr, settings) {

        if (settings.headers != undefined) {
            let isBackgroundTask = settings.headers["x-backgroup-task-header"];

            if (isBackgroundTask == undefined || isBackgroundTask == false) {
                $.blockUI();
            }

        } else {
            $.blockUI();
        }




    }).ajaxStop($.unblockUI);
} catch (e) {
    console.log("blockUI 💣");
}




//Copy To Clipboard fix.
window.Clipboard = (function (window, document, navigator) {
    var textArea,
        copy;

    function isOS() {
        return navigator.userAgent.match(/ipad|iphone/i);
    }

    function createTextArea(text) {
        textArea = document.createElement('textArea');
        textArea.value = text;
        document.body.appendChild(textArea);
    }

    function selectText() {
        var range,
            selection;

        if (isOS()) {
            range = document.createRange();
            range.selectNodeContents(textArea);
            selection = window.getSelection();
            selection.removeAllRanges();
            selection.addRange(range);
            textArea.setSelectionRange(0, 999999);
        } else {
            textArea.select();
        }
    }

    function copyToClipboard() {
        document.execCommand('copy');
        document.body.removeChild(textArea);
    }

    copy = function (text) {
        createTextArea(text);
        selectText();
        copyToClipboard();
    };

    return {
        copy: copy
    };
})(window, document, navigator);



$(function () {

    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        language: 'tr-TR'
    });
    runwysihtml5();
    runSelect2();

});

function runwysihtml5() {
    $('.wysihtml5').wysihtml5();
}

function runSelect2() {
    // select2
    $('.select2').select2();

    $('.select2-ajax-data').each(function () {
        debugger;
        let url = $(this).data().url;
        let select = $(this);
        var selectedValue = $(this).data().selected;
        $(this)
            .find('option')
            .remove()
            .end();
        $.ajax({
            url: url,
            dataType: 'json',
            type: 'GET',
            success: function (result) {
                for (var i = 0; i < result.length; i++) {
                    $(select).append(new Option(result[i].Value, result[i].Idx, result[i].Idx == selectedValue));
                }
                $(select).val(selectedValue);
                $(select).trigger('change');
            }
        });
        $(this).select2();
    })
}

function runSelect2inModal() {
    // select2
    $('.select2').select2(
        {
            dropdownParent: $('#myModal')
        });


    $('.select2-ajax-data').each(function () {
        debugger;
        let url = $(this).data().url;
        let select = $(this);
        var selectedValue = $(this).data().selected;
        $(this)
            .find('option')
            .remove()
            .end();
        $.ajax({
            url: url,
            dataType: 'json',
            type: 'GET',
            success: function (result) {
                for (var i = 0; i < result.length; i++) {
                    $(select).append(new Option(result[i].Value, result[i].Idx, result[i].Idx == selectedValue));
                }
                $(select).val(selectedValue);
                $(select).trigger('change');
            }
        });
        $(this).select2({
            dropdownParent: $('#myModal')
        });
    })

}

function FieldListBox() {
    $('.list-ajax-data').each(function () {
        debugger;
        let url = $(this).data().url;
        let ul = $(this);
        console.log(url);
        $(this)
            .find('li')
            .remove()
            .end();
        $.ajax({
            url: url,
            dataType: 'json',
            type: 'GET',
            success: function (result) {
                for (var i = 0; i < result.length; i++) {
                    var li = '';
                    if (result[i].TypeIdx == 1) {

                        li = '<li class="list-group-item" data-Idx="' + result[i].Idx + '">';
                        li += '<div class="row">';
                        li += '<div class="col-md-12">';
                        li += ' <div class="form-group">';
                        li += ' <label>' + result[i].Name_TR + '</label>';
                        li += '<input type="text" class="form-control"/>';
                        li += '<label> Zorunlu: ' + result[i].IsRequired + ' Max Length : ' + result[i].MaxLength + ' Min Length: ' + result[i].MinLength + '</label>';
                        li += '</div>';
                        li += '</div>';
                        li += '</li>';
                    } else if (result[i].TypeIdx == 2) {
                        li = '<li class="list-group-item" data-Idx="' + result[i].Idx + '">';
                        li += '<div class="row">';
                        li += '<div class="col-md-12">';
                        li += ' <div class="form-group">';
                        li += ' <label>' + result[i].Name_TR + '</label>';
                        li += '<input type="number" min=' + result[i].MinLength + ' max=' + result[i].MaxLength + ' class="form-control"/>';
                        li += '<label> Zorunlu: ' + result[i].IsRequired + ' Max Length : ' + result[i].MaxLength + ' Min Length: ' + result[i].MinLength + '</label>';
                        li += '</div>';
                        li += '</div>';
                        li += '</li>';
                    } else if (result[i].TypeIdx == 4) {
                        li = '<li class="list-group-item" data-Idx="' + result[i].Idx + '">';
                        li += '<div class="row">';
                        li += '<div class="col-md-12">';
                        li += ' <div class="form-group">';
                        li += ' <label>' + result[i].Name_TR + '</label>';
                        li += '<input type="date" class="form-control"/>';
                        li += '<label> Zorunlu: ' + result[i].IsRequired + ' Max Length : ' + result[i].MaxLength + ' Min Length: ' + result[i].MinLength + '</label>';
                        li += '</div>';
                        li += '</div>';
                        li += '</li>';
                    } else if (result[i].TypeIdx == 5) {
                        li = '<li class="list-group-item" data-Idx="' + result[i].Idx + '">';
                        li += '<div class="row">';
                        li += '<div class="col-md-12">';
                        li += ' <div class="form-group">';
                        li += ' <label>' + result[i].Name_TR + '</label>';
                        li += '<input type="checkbox"/></br>';
                        li += '<label> Zorunlu: ' + result[i].IsRequired + ' Max Length : ' + result[i].MaxLength + ' Min Length: ' + result[i].MinLength + '</label>';
                        li += '</div>';
                        li += '</div>';
                        li += '</li>';
                    }
                    else if (result[i].TypeIdx == 6) {
                        li = '<li class="list-group-item" data-Idx="' + result[i].Idx + '">';
                        li += '<div class="row">';
                        li += '<div class="col-md-12">';
                        li += ' <div class="form-group">';
                        li += ' <label>' + result[i].Name_TR + '</label>';
                        li += '<textarea row="5" class="form-control"/>';
                        li += '<label> Zorunlu: ' + result[i].IsRequired + ' Max Length : ' + result[i].MaxLength + ' Min Length: ' + result[i].MinLength + '</label>';
                        li += '</div>';
                        li += '</div>';
                        li += '</li>';
                    }
                    else {
                        li = '<li class="list-group-item" data-Idx="' + result[i].Idx + '">';
                        li += '<div class="row">';
                        li += '<div class="col-md-8">';
                        li += result[i].Name_TR;
                        li += '</div>';
                        li += '<div class="col-md-4">';
                        li += result[i].TypeName;
                        li += '</div>';
                        li += '</div>';
                        li += '<div class="row">';
                        li += '<div class="col-md-12">';
                        li += ' Zorunlu: ' + result[i].IsRequired + ' Max Length : ' + result[i].MaxLength + ' Min Length: ' + result[i].MinLength;
                        li += '</div>';
                        li += '</div>';
                        li += '</li>';
                    }

                    console.log(li);
                    $(ul).append(li);
                }
            }
        });

    });
    $('[name="SearchDualList"]').keyup(function (e) {
        var code = e.keyCode || e.which;
        if (code == '9') return;
        if (code == '27') $(this).val(null);
        var $rows = $(this).closest('.dual-list').find('.list-group li');
        var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
        $rows.show().filter(function () {
            var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
            return !~text.indexOf(val);
        }).hide();
        return false;
    });
}

function objectifyform(formarray) {
    var returnarray = {};
    for (var i = 0; i < formarray.length; i++) {

        if (formarray[i]['name'] in returnarray) {
            if (Array.isArray(returnarray[formarray[i]['name']])) {
                returnarray[formarray[i]['name']].push(formarray[i]['value']);
            } else {

                tempvar = returnarray[formarray[i]['name']];
                returnarray[formarray[i]['name']] = [];
                returnarray[formarray[i]['name']].push(tempvar, formarray[i]['value']);

            }
        } else {
            returnarray[formarray[i]['name']] = formarray[i]['value'];
        }
    }
    return returnarray;
}


function GetProductFields(url, isEdit, fillUrl) {

    $.ajax({
        url: url,
        dataType: 'json',
        type: 'GET',
        success: function (result) {
            $('#productFields').html('');
            for (var i = 0; i < result.length; i++) {
                var li = '';
                if (result[i].TypeIdx == 1) {
                    li += ' <div class="form-group">';
                    li += ' <label>' + result[i].Name_TR + '</label>';
                    li += '<input type="text" name=' + result[i].Idx + ' id = ' + result[i].Idx + ' class="form-control" data-required="' + result[i].IsRequired + '"/>';
                    li += '</div>';
                } else if (result[i].TypeIdx == 2) {
                    li += ' <div class="form-group">';
                    li += ' <label>' + result[i].Name_TR + '</label>';
                    li += '<input type="number" min=' + result[i].MinLength + ' max=' + result[i].MaxLength + ' name=' + result[i].Idx + ' id = ' + result[i].Idx + ' class="form-control" data-required="' + result[i].IsRequired + '"/>';

                    li += '</div>';

                } else if (result[i].TypeIdx == 4) {
                    li += ' <div class="form-group">';
                    li += ' <label>' + result[i].Name_TR + '</label>';
                    li += '<input type="date" name=' + result[i].Idx + ' id = ' + result[i].Idx + '  class="form-control" data-required="' + result[i].IsRequired + '"/>';
                    li += '</div>';

                } else if (result[i].TypeIdx == 5) {
                    li += ' <div class="form-group">';
                    li += ' <label>' + result[i].Name_TR + '</label>';
                    li += '<input type="checkbox"  name=' + result[i].Idx + ' id = ' + result[i].Idx + ' data-required="' + result[i].IsRequired + '" /></br>';
                    li += '</div>';
                }
                else if (result[i].TypeIdx == 6) {
                    li += ' <div class="form-group">';
                    li += ' <label>' + result[i].Name_TR + '</label>';
                    li += '<textarea row="5" name=' + result[i].Idx + ' id = ' + result[i].Idx + ' data-required="' + result[i].IsRequired + '"  class="form-control"/>';
                    li += '</div>';
                }
                else {
                    li = '<li class="list-group-item" data-Idx="' + result[i].Idx + '">';
                    li += '<div class="row">';
                    li += '<div class="col-md-8">';
                    li += result[i].Name_TR;
                    li += '</div>';
                    li += '<div class="col-md-4">';
                    li += result[i].TypeName;
                    li += '</div>';
                    li += '</div>';
                    li += '<div class="row">';
                    li += '<div class="col-md-12">';
                    li += ' Zorunlu: ' + result[i].IsRequired + ' Max Length : ' + result[i].MaxLength + ' Min Length: ' + result[i].MinLength;
                    li += '</div>';
                    li += '</div>';
                    li += '</li>';
                }

                $('#productFields').append(li);
            }
            if (isEdit) {
                FillProductFields(fillUrl);
            }
        }
    });
}

function FillProductFields(fillurl) {
    $.ajax({
        url: fillurl,
        method: "GET",
        success: function (resp) {

            resp.forEach(function (item) {
                $('#' + item.FieldIdx).val(item.Value);
            })

        },
        error: function (err) {
            Toast.fire({
                type: 'error',
                title: ("<h3>" + resp.Message + "</h3>")
            })
        }
    })
}

var elem = document.getElementById("sidebar-search-q");

elem.addEventListener("keyup", searchInMenu)


function searchInMenu(event) {
    debugger;

    var searchResultContainer = $("#sidebar-menu-search-result");
    var searchContainer = $("#sidebar-menu-container");
    searchResultContainer.html("");
    
    if (elem.value.length > 0) {
        searchContainer.addClass("hidden");

        var filteredBar = $(searchContainer).find("a").filter(function (key, val) {
            return val.classList.length > 0;
        })

        $.each(filteredBar, function (k, val) {
            var itemToSearch = val.text.toLowerCase();
            var textToFind = elem.value.toLowerCase();
            if (itemToSearch.indexOf(textToFind) >= 0) {
                let itemToAppend = val.closest("li").outerHTML;
                searchResultContainer.append(itemToAppend);
            }
        });

        searchResultContainer.removeClass("hidden");

    } else {
        searchContainer.removeClass("hidden");
        searchResultContainer.addClass("hidden");
        
    }
}