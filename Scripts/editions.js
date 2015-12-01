$(function () {
    $('#checkoutModal').on('show.bs.modal', function (e) {
        var edition = $(e.relatedTarget).attr("data-edition");
        
        if (edition)
        {
            $('#edition_selected').val(edition);
            SetPrice(edition);
        } else {
            ResetPrice();
        }
    })

    $('#edition_selected').on('change', function () {
        if (this.value == "ProLite") SetPrice("ProLite");
        else if (this.value == "ProFull") SetPrice("ProFull");
        else if (this.value == "EntLite") SetPrice("EntLite");
        else if (this.value == "EntFull") SetPrice("EntFull");
        else if (this.value == "Leader") SetPrice("Leader");
        else ResetPrice();
    });

    function SetPrice(ed)
    {
        if (ed == "ProLite") {
            $('#edition_description').val("Professional Lite Edition");
            $('#edition_price').val("49");
        }
        else if (ed == "ProFull") {
            $('#edition_description').val("Professional Edition");
            $('#edition_price').val("99");
        }
        else if (ed == "EntLite") {
            $('#edition_description').val("Enterprise Lite Edition");
            $('#edition_price').val("149");
        }
        else if (ed == "EntFull") {
            $('#edition_description').val("Enterprise Edition");
            $('#edition_price').val("199");
        }
        else if (ed == "Leader") {
            $('#edition_description').val("Global Leader Edition");
            $('#edition_price').val("499");
        }
        else ResetPrice();
    }

    function ResetPrice()
    {
        $('#edition_price').val("ProLite");
        $('#edition_description').val("Professional Lite Edition");
        $('#edition_price').val("49");
    }

    var validateTimer = undefined;
    $('#checkoutModal').on('click', '#buttonOrder', function (e) {
        var edition = $('#edition_selected').val();
        SetPrice(edition);
        var price = $('#edition_price').val();

        if (edition && price) {
            var name = $('#name_customer').val();
            if (name.length < 5) {
                $('#name_customer').val("");
                name = "";
            }

            var email = $('#email_customer').val();
            if ((email.length < 8) || (validateEmail(email) == false)) {
                $('#email_customer').val("");
                email = "";
            }

            if ((name != "") && (email != "")) {
                e.preventDefault();

                setTimeout(function() {$('#checkoutModal').modal('hide')}, 30000);
                $('.modal-dialog').hide(function () {
                    var customer = $('#name_customer').val() + " (" + $('#email_customer').val() + ")";
                    var edition = $('#edition_selected').val() + " (" + $('#edition_price').val() + ")";
                    var company = $('#company_customer').val();
                    var message = $('#message_customer').html();
                    if (message.length > 100) message = message.substr(0, 100);
                    var gaCust = customer + " - " + edition + " - " + company + " - " + message + " (checkout started)";
                    ga('send', 'pageview', {
                        'dimension1': gaCust
                    });
                    setTimeout(function () { $('#checkoutForm').submit() }, 1000);
                });
            } else {
                if (typeof(validateTimer) !== "undefined") validateTimer = clearTimeout(validateTimer);
                $('#msgValidate').show();
                validateTimer = setTimeout(function () { $('#msgValidate').hide(); }, 5000);
            }

        } else {
            ResetPrice();
        }

    });

    function validateEmail(email) {
        var re = /\S+@\S+\.\S+/;
        return re.test(email);
    }
    $('#edition_selected,#email_customer,#name_customer,#message_customer').tooltip();

})


function parseCurrency(result) {
    if (result.query.results == null) return;

    try {
        var date = new Date(result.query.created);
        $('#time-rates').html("<sup>*</sup> The exchange rates are calculated for informational purposes only. The validity can not be guaranteed. Source: Yahoo Finance from " + date.toString());

        var usdrate = result.query.results.body.p;
        usdrate = usdrate.replace("\"EURUSD=X\",", "");
        var usd = Number(usdrate);
        var $ed = $('#editions');
        var sum = Math.floor(49 * usd);
        $ed.find('#usd-prolite').text(sum);
        sum = Math.floor(99 * usd);
        $ed.find('#usd-pro').text(sum);
        sum = Math.floor(149 * usd);
        $ed.find('#usd-entlite').text(sum);
        sum = Math.floor(199 * usd);
        $ed.find('#usd-ent').text(sum);
        sum = Math.floor(499 * usd);
        $ed.find('#usd-leader').text(sum);
    }
    catch (e)
    {
        $('#time-rates').text("");
    }
}

$('document').ready(function () {

    if (window.location.href.indexOf('note=checkout_success') > 0) {
        $('.alert-success').show();
        ga('send', 'pageview', {
            'dimension1': 'checkout success'
        });
    }
    else if (window.location.href.indexOf('note=checkout_canceled') > 0) {
        $('.alert-danger').show();
        ga('send', 'pageview', {
            'dimension1': 'checkout canceled'
        });
    }

    $('#footer-mail').attr("href", "mailto:backload.org@gmail.com").text("backload.org@gmail.com");

    $.getScript("http://query.yahooapis.com/v1/public/yql?q=select%20p%20from%20html%20where%20url%3D'http%3A%2F%2Fdownload.finance.yahoo.com%2Fd%2Fquotes.txt%3Fs%3DEURUSD%3DX%26f%3Dsl1'&format=json&callback=parseCurrency");
});
