// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    var selectedDates = []; // Tablica do przechowywania zaznaczonych dat

    // Obsługa zdarzenia kliknięcia na dniu kalendarza
    $('.fc-day').click(function () {
        var clickedDate = $(this).attr('data-date');

        // Sprawdzamy, czy kliknięty dzień jest już zaznaczony
        if (selectedDates.includes(clickedDate)) {
            $(this).removeClass('selected'); // Usuwamy klasę selected, aby odznaczyć dzień
            selectedDates = selectedDates.filter(date => date !== clickedDate); // Usuwamy kliknięty dzień z tablicy
        } else {
            if (selectedDates.length < 2) { // Sprawdzamy, czy użytkownik zaznaczył już dwie daty
                // Dodajemy klasę selected, aby zaznaczyć dzień
                $(this).addClass('selected');
                selectedDates.push(clickedDate); // Dodajemy kliknięty dzień do tablicy
            }
        }
    });

    $('#showWeatherButton').click(function () {
        var city = $('#cityInput').val();

        // Wyślij żądanie AJAX do akcji "GetSelectedDatesAndCity" z miastem i wybranymi datami podróży
        $.ajax({
            url: '/Calendar/GetSelectedDatesAndCity',
            method: 'post',
            data: { city: city, selectedDates: selectedDates },
            success: function (data) {
                // Wyświetlenie otrzymanych danych w konsoli
                console.log('Otrzymane dane:', data);

                // Tutaj możemy przetwarzać otrzymane dane, np. wyświetlić je na stronie
            },
            error: function (xhr, status, error) {
                // Obsługa błędu
                console.error(error);
            }
        });
    });
});
