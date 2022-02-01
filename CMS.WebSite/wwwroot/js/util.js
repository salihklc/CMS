$("body").delegate(".remove-btn-ajax", "click", function (eve) {
    eve.preventDefault();
    var $this = $(this);
    var removeLink = $this.attr("href");
    var title = $this.attr("data-remove-title") ? $this.attr("data-remove-title") : "Emin Misiniz?";
    var text = $this.attr("data-remove-text") ? $this.attr("data-remove-text") : "Bu kaydı silmek istediğinize emin misiniz?";
    var confirmButtonText = $this.attr("data-remove-confirmButtonText") ? $this.attr("data-remove-confirmButtonText") : "Evet";
    var cancelButtonText = $this.attr("data-remove-cancelButtonText") ? $this.attr("data-remove-cancelButtonText") : "Hayır";
    var confirmTitle = $this.attr("data-remove-confirmTitle") ? $this.attr("data-remove-confirmTitle") : "Başarılı";
    var confirmMessage = $this.attr("data-remove-confirmMessage") ? $this.attr("data-remove-confirmMessage") : "Silme İşlemi Başarılı";
    var cancelTitle = $this.attr("data-remove-cancelTitle") ? $this.attr("data-remove-cancelTitle") : "Başarısız";
    var cancelMessage = $this.attr("data-remove-cancelMessage") ? $this.attr("data-remove-cancelMessage") : "Silme İşlemi Başarısız";

    Swal.fire({
        title: title,
        text: text,
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText
    }).then((result) => {

        $.ajax({
            'url': removeLink,
            'method': "POST",
            'success': function (resp) {
                if (resp.IsSuccess) {
                    Swal.fire(
                        confirmTitle,
                        confirmMessage,
                        'success'
                    ).then((then) => {
                        location.reload();
                    })
                    // For more information about handling dismissals please visit
                    // https://sweetalert2.github.io/#handling-dismissals
                } else {
                    Swal.fire(
                        cancelTitle,
                        cancelMessage,
                        'error'
                    )
                }
            },
            'error': function (err) {
                if (err.status == 403) {
                    cancelMessage = "Bu İşlemi Yapmaya Yetkiniz Bulunmamaktadır.";
                }
                Swal.fire(
                    cancelTitle,
                    cancelMessage,
                    'error'
                )
            }
        });

    })

});

var successPartialInıtJs = [];

$("body").delegate(".ajax-form", "submit", function (eve) {
    eve.preventDefault();
    
    var isModal = $(this).data().ismodal;
    $(this).ajaxSubmit(function (resp) {
        $.blockUI();
    
        if (resp.IsSuccess) {
            Swal.fire(
                resp.Message,
                resp.Message,
                'success'
            ).then((then) => {
                if (resp.RedirectUrl == undefined ) {
                    location.reload();
                } else {
                    location.href = resp.RedirectUrl;
                }
                
            })
        } else {
            if (isModal) {
                $('#modalBody').html(resp);
                $('#myModal').modal();
                // Bunları daha düzgün yapacam
                runwysihtml5();
                runSelect2();
            } else {
                $("section.content").html(resp);
            }
           

            if (successPartialInıtJs.length > 0) {
                $.each(successPartialInıtJs, function (k, v) {
                    v();
                });
            }
        }
    });

    return false;
});


var settingsbarSelectLanguage = $('#settingsbar-select-language');

let reqGetLangUrl = baseAdminPageUrl + "/Configuration/GetLanguage";

$.ajax({
    "url": reqGetLangUrl,
    "method": "get",
    "success": function (response) {
        if (response.length > 0) {
            var html = "";
            response.forEach(element => {
                html += (`<option value='${element.Value}' ${element.Selected ? "selected":""}>${element.Text}</option>`);
            });
            settingsbarSelectLanguage.html(html);
        }
    }
})

settingsbarSelectLanguage.select2({
    minimumResultsForSearch: -1
});

settingsbarSelectLanguage.on("select2:select", function (e) {
    debugger;
    let reqUrl = baseAdminPageUrl + "/Configuration/SetLanguage";

    let data = {
        LocalizationCode: settingsbarSelectLanguage.val()
    };

    $.ajax({
        "url": reqUrl,
        "method": "post",
        "data": data,
        "success": function (resp) {
            if (resp.IsSuccess) {
                Toast.fire({
                    type: 'success',
                    title: ("<h3>" + resp.Message + "</h3>")
                })
            } else {
                Toast.fire({
                    type: 'error',
                    title: ("<h3>" + resp.Message + "</h3>")
                })
            }
        },
        "error": function (err) {
            Toast.fire({
                type: 'error',
                title: ("<h3>" + resp.Message + "</h3>")
            })
        },
        "complete": function (cmp) { }
    })
});

function OpenModal(url) {
    debugger;
    $.ajax({
        type: 'GET',
        url:url,
        success: function (resultData) {
            debugger;
            $('#modalBody').html(resultData);
            $('#myModal').modal();
            runwysihtml5();
            runSelect2();
        },
        error: function (xhr, status, error) {
            var errorMessage = xhr.status + ': ' + xhr.statusText
            console.log(errorMessage);
            if (xhr.status == 403) {
                Swal.fire(
                    "Başarısız",
                    "Bu İşlemi Yapmaya Yetkiniz Bulunmamaktadır.",
                    'error'
                )
            }            
            else if (xhr.status != 200) {
                CookieTimeOut();
            }
        }
    });
  
}
function OpenModalPost(url, data) {
    debugger;
    $.ajax({
        type: 'post',
        url: url,
        data: {model:data},
        success: function (resultData) {
            debugger;
            $('#modalBody').html(resultData);
            $('#myModal').modal();
            runwysihtml5();
            runSelect2inModal();
        },
        error: function (xhr, status, error) {
            var errorMessage = xhr.status + ': ' + xhr.statusText
            console.log(errorMessage);
            if (xhr.status == 403) {
                Swal.fire(
                    "Başarısız",
                    "Bu İşlemi Yapmaya Yetkiniz Bulunmamaktadır.",
                    'error'
                )
            }
            else if (xhr.status != 200) {
                CookieTimeOut();
            }
        }
    });
}

function CookieTimeOut() {
    Swal.fire(
        "Bilgilendirme",
        "Uzun Süre İşlem Yapmadığınız İçin Oturumunuz Sonlandırılıyor.",
        'info'
    ).then(() => {
        window.location.href = logoutUrl;
    });
}

