$(document).ready(function () {
    $.getJSON("http://openexchangerates.org/api/currencies.json?app_id=83199c2d40c34d21842aa05702e1cadc", function (result) {
        var $optionString = "";
        for (var $key in result) {
            $optionString += "<option value=" + $key;
            if ($key == "USD") {
                $optionString += " selected";
            }
            $optionString += ">" + result[$key] + "</option>"
        }
        $("#currencies").html($optionString);
    });

    $("#currencies").change(function () {
        $.getJSON("http://openexchangerates.org/api/latest.json?app_id=83199c2d40c34d21842aa05702e1cadc", function (result) {
            var currency = $("#currencies").val();

            var exchangeRate = parseFloat(result.rates[currency]);

            var $priceFields = $(".price");


            for (var i = 0; i < $priceFields.length; i++) {

                var oldPrice = parseFloat($priceFields.eq(i).attr('title'));

                var newPrice = (oldPrice * exchangeRate / 100.0).toFixed(2);
                if (newPrice != 0) {
                    $priceFields.eq(i).text(newPrice);
                }
            }
        });
    });
});