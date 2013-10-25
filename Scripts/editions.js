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
                    var val = $('#name_customer').val() + "|";
                    val = val + $('#email_customer').val() + "|";
                    val = val + $('#company_customer').val() + "|";
                    val = val + $('#edition_selected').val() + "|";
                    val = val + $('#edition_price').val() + "|";
                    val = val + $('#message_customer').html();
                    _gaq.push(['_setCustomVar', 1, 'Checkout', val, 3]);
                    _gaq.push(['_trackEvent', 'Ckeckout', 'Started']);

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

$('document').ready(function () {

    if (window.location.href.indexOf('note=checkout_success') > 0) {
        $('.alert-success').show();
        _gaq.push(['_setCustomVar', 2, 'Checkout', 'Success', 3]);
        _gaq.push(['_trackEvent', 'Ckeckout', 'Finished']);
    }
    else if (window.location.href.indexOf('note=checkout_canceled') > 0) {
        $('.alert-danger').show();
        _gaq.push(['_setCustomVar', 2, 'Checkout', 'Canceled', 3]);
        _gaq.push(['_trackEvent', 'Ckeckout', 'Finished']);
    }

    $('#footer-mail').attr("href", "mailto:backload.mvc@gmail.com").text("backload.mvc@gmail.com");
});