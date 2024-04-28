// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    var startDate = null; // Zmienna do przechowywania daty początkowej
    var endDate = null; // Zmienna do przechowywania daty końcowej

    // Obsługa zdarzenia kliknięcia na dniu kalendarza
    $('.fc-day').click(function () {
        var clickedDate = $(this).attr('data-date');

        if (startDate === null) {
            // Jeśli nie ma jeszcze daty początkowej, ustawiamy ją jako klikniętą datę
            startDate = clickedDate;
            $(this).addClass('selected-start');
        } else if (endDate === null) {
            // Jeśli jest już ustawiona data początkowa, ale nie ma jeszcze daty końcowej
            if (clickedDate > startDate) {
                // Jeśli kliknięta data jest późniejsza niż data początkowa, ustawiamy ją jako datę końcową
                endDate = clickedDate;
                $(this).addClass('selected-end');
            } else {
                // Jeśli kliknięta data jest wcześniejsza niż data początkowa, zamieniamy daty miejscami
                endDate = startDate;
                startDate = clickedDate;
                $(this).addClass('selected-start');
                $('.selected-end').removeClass('selected-end');
            }
        } else {
            // Jeśli obie daty są już ustawione, resetujemy je i ustawiamy nową datę początkową jako klikniętą datę
            startDate = clickedDate;
            endDate = null;
            $('.selected-start').removeClass('selected-start');
            $('.selected-end').removeClass('selected-end');
            $(this).addClass('selected-start');
        }
    });

    $('#showWeatherButton').click(function () {
        var city = $('#cityInput').val();

        if (startDate !== null && endDate !== null) {
            // Wyślij żądanie AJAX do akcji "GetSelectedDatesAndCity" z miastem, datą początkową i datą końcową
            $.ajax({
                url: '/Calendar/GetSelectedDatesAndCity',
                method: 'post',
                data: { city: city, startDate: startDate, endDate: endDate },
                success: function (data) {
                    console.log('Otrzymane dane:', data);

                    // Wyświetlenie  listy ubrań do wzięcia
                    $('#weatherDetailsContainer').append('<h2>Lista ubrań, które należy wziąć:</h2>');
                    data.clothesByDate.forEach(function (clothes) {
                        $('#weatherDetailsContainer').append('<p>' + clothes + '</p>');
                    });
                    $('#weatherDetailsContainer').append('<h2>Ubrania, których brakuje i należałoby je dokupić:</h2>');
                    data.clothesBuy.forEach(function (clothes) {
                        $('#weatherDetailsContainer').append('<p>' + clothes + '</p>');
                    });
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        } else {
            console.log('Wybierz datę początkową i końcową podróży.');
        }
    });
});



