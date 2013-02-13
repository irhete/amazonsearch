$(document).ready(function () {
    $("#currencies").change(function () {
        $.getJSON("http://openexchangerates.org/api/latest.json?app_id=83199c2d40c34d21842aa05702e1cadc", function (result) {
            var currency = $("#currencies").val();

            var exchangeRate = parseFloat(result.rates[currency]);

            var $priceFields = $(".price");


            for (var i = 0; i < $priceFields.length; i++) {

                var oldPrice = parseFloat($priceFields.eq(i).attr('title'));

                var newPrice = (oldPrice * exchangeRate / 100.0).toFixed(2);

                $priceFields.eq(i).text(newPrice);
            }
        });
    });
});