// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// kod funkcji walidującej NIP pochodzi z https://pl.wikibooks.org/wiki/Kody_%C5%BAr%C3%B3d%C5%82owe/Implementacja_NIP#Implementacja_algorytmu_w_j%C4%99zyku_JavaScript
function NIPIsValid(nip) {
    var weights = [6, 5, 7, 2, 3, 4, 5, 6, 7];
    nip = nip.replace(/[\s-]/g, '');

    if (nip.length === 10 && parseInt(nip, 10) > 0) {
        var sum = 0;
        for (var i = 0; i < 9; i++) {
            sum += nip[i] * weights[i];
        }
        return (sum % 11) === Number(nip[9]);
    }
    return false;
}

$('#nip').change(function () {
    var input = $('#nip').val();
    var info = '';
    if (NIPIsValid(input)) {
        $('#submitBtn').prop('disabled', false);
        info = '';
    }
    else {
        info = 'NIP jest niepoprawny!'; 
        $('#submitBtn').prop('disabled', true);
    }    
    $('#nipValidation').text(info);
});