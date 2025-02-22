let currentPage = 1;

function removeFocus() {
    var activeElement = $(':focus');
    activeElement.blur();
}

function loadHotels(page) {
    var latitude = $('#latitude').val();
    var longitude = $('#longitude').val();

    console.log(page, latitude, longitude);
    $.get(`/api/hotels?page=${page}&latitude=${latitude}&longitude=${longitude}`, function (data) {
        let rows = '';
        $.each(data.hotels, function (index, hotel) {
            rows += `<tr>
                        <td>${hotel.id}</td>
                        <td>${hotel.name}</td>
                        <td>${hotel.price}</td>
                        <td>${hotel.latitude}</td>
                        <td>${hotel.longitude}</td>
                        <td>${hotel.distance}</td>
                        <td>
                            <button class='btn btn-primary' onclick='openEditModal(${hotel.id}, "${hotel.name}", ${hotel.price}, ${hotel.latitude}, ${hotel.longitude})'>Update</button>
                        </td>
                        <td>
                            <button class='btn btn-danger' onclick='deleteHotel(${hotel.id})'>Delete</button>
                        </td>
                    </tr>`;
        });

        $('#hotelTableBody').html(rows);
        updatePagination(data.totalPages);
    });
}

function updatePagination(totalPages) {
    let paginationHtml = '';
    for (let i = 1; i <= totalPages; i++) {
        paginationHtml += `<li class='page-item ${i === currentPage ? "active" : ""}'>
                    <a class='page-link' href='#' onclick='changePage(${i})'>${i}</a>
                </li>`;
    }
    $('#pagination').html(paginationHtml);
}

function changePage(page) {
    currentPage = page;
    loadHotels(currentPage);
}

function openCreateModal() {
    $('#createName').val('');
    $('#createPrice').val('');
    $('#createLatitude').val('');
    $('#createLongitude').val('');
    removeFocus();
    $('#createModal').modal('show');
}

function closeEditModal() {
    $('#editModal').modal('hide');
}

function closeCreateModal() {
    $('#createModal').modal('hide');
}

function createHotel() {
    const newHotel = {
        name: $('#createName').val(),
        price: parseFloat($('#createPrice').val()),
        latitude: parseFloat($('#createLatitude').val()),
        longitude: parseFloat($('#createLongitude').val())
    };

    $.ajax({
        url: `/api/hotels`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(newHotel),
        success: function () {
            $('#createModal').modal('hide');
            loadHotels(currentPage);
        }
    });
}

function openEditModal(id, name, price, latitude, longitude) {
    $('#editHotelId').val(id);
    $('#editName').val(name);
    $('#editPrice').val(price);
    $('#editLatitude').val(latitude);
    $('#editLongitude').val(longitude);
    removeFocus();
    $('#editModal').modal('show');
}

function saveChanges() {
    const id = $('#editHotelId').val();
    const updatedHotel = {
        name: $('#editName').val(),
        price: parseFloat($('#editPrice').val()),
        latitude: parseFloat($('#editLatitude').val()),
        longitude: parseFloat($('#editLongitude').val())
    };

    $.ajax({
        url: `/api/hotels/${id}`,
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(updatedHotel),
        success: function () {
            $('#editModal').modal('hide');
            loadHotels(currentPage);
        }
    });
}

function deleteHotel(id) {
    $.ajax({
        url: `/api/hotels/${id}`,
        type: 'DELETE',
        success: function () {
            loadHotels(currentPage);
        }
    });
}

$(document).ready(function () {
    loadHotels(currentPage);
});