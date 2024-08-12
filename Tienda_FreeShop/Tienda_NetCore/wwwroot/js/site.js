function showInPopup(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-body').html(res);
            $('#form-modal-title').html(title);
            $('#form-modal').modal('show');
        },
        error: function (xhr, status, error) {
            console.error("Error en la solicitud AJAX:", status, error);
            $('#form-modal-body').html('<p>Error al cargar el contenido.</p>');
            $('#form-modal-title').html('Error');
            $('#form-modal').modal('show');
        }
    });
}
function jQueryAjaxPost(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    // Si es válido, redirige o haz algo más
                    location.href = '@Url.Action("Login", "Cuenta")'; // Redirige a Login en caso de éxito
                } else {
                    // Si no es válido, muestra los errores de validación
                    $('#form-modal .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
        // Prevenir el envío del formulario por defecto
        return false;
    } catch (ex) {
        console.log(ex);
    }
}

function jQueryAjaxPost(form) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: $(form).serialize(),
            success: function (res) {
                if (res.isValid) {
                    // Redirige si la autenticación es exitosa
                    window.location.href = res.redirectUrl || '/';
                } else {
                    // Muestra los errores de validación
                    alert(res.errorMessage);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
        return false; // Prevenir el envío del formulario por defecto
    } catch (ex) {
        console.log(ex);
    }
}

$(document).ready(function () {
    $('#login-form').on('submit', function (e) {
        e.preventDefault();
        return jQueryAjaxPost(this);
    });
});


