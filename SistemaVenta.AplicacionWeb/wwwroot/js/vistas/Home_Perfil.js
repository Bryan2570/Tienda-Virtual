


$(document).ready(function () {

    $(".container-fluid").LoadingOverlay("show");

    fetch("/Home/ObtenerUsuario")
        .then(response => {
            $(".container-fluid").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {


            if (responseJson.estado) {
                const d = responseJson.objeto

                $("#imgFoto").attr("src", d.urlFoto)
                $("#txtNombre").val(d.nombre)
                $("#txtCorreo").val(d.correo)
                $("#txtTelefono").val(d.telefono)
                $("#txtRol").val(d.nombreRol)

            } else {
                swal("Lo sentimos", responseJson.mensaje, "error")
            }
        })
})

$("#btnGuardarCambios").click(function () {

    console.log($("#txtTelefono").val().trim())


    if ($("#txtCorreo").val().trim() == "") {
        toastr.warning("", "Debe completar el campo Correo")
        $("#txtCorreo").focus()
        return;
    }

    if ($("#txtTelefono").val().trim() == "") {
        toastr.warning("", "Debe completar el campo telefono")
        $("#txtTelefono").focus()
        return;
    }

    swal({
        title: "¿Desea guardar los cambios?",
        
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Si",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true
    },

        function (respuesta) {
            //si pressiona si devuelve true y entra aca 
            if (respuesta) {

                $(".showSweetAlert").LoadingOverlay("show");

                let modelo = {
                    correo: $("#txtCorreo").val().trim(),
                    telefono: $("#txtTelefono").val().trim()
                }

                console.log(modelo)

                fetch("/Home/GuardarPerfil", {
                    method: "POST",
                    headers: { "Content-Type": "application/json;charset=utf-8" },
                    body: JSON.stringify(modelo)
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {
                            swal("Listo!", "Los cambios fueron guardados", "success")
                        } else {
                            swal("Lo sentimos", responseJson.mensaje, "error")
                        }
                    })
            }

        }
    )
})


$("#btnCambiarClave").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    //console.log(inputs)
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "");
    //console.log(inputs_sin_valor)

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo : "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }


    if ($("#txtClaveNueva").val().trim() != $("#txtConfirmarClave").val().trim()) {
        toastr.warning("", "Las contraseñas no coinciden")
        $("#txtCorreo").focus()
        return;
    }

    let modelo = {
        claveActual: $("#txtClaveActual").val().trim(),
        claveNueva: $("#txtClaveNueva").val().trim()
    }


    fetch("/Home/CambiarClave", {
        method: "POST",
        headers: { "Content-Type": "application/json;charset=utf-8" },
        body: JSON.stringify(modelo)
    })
        .then(response => {
            $(".showSweetAlert").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {

            if (responseJson.estado) {
                swal("Listo!", "La contraseña fue actualizada correctamente", "success")
                $("input.input-validar").val(""); //limpiamos el contenido de la caja de texto
            } else {
                swal("Lo sentimos", responseJson.mensaje, "error")
            }
        })
});