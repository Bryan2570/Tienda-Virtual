$(document).ready(function () {

    fetch("/Venta/ListaTipoDocumentoVenta")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboTipoDocumentoVenta").append(
                        $("<option>").val(item.IdTipoDocumentoVenta).text(item.descripcion)
                    )

                })
            }
        })



    fetch("/Negocio/Obtener")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {

            if (responseJson.estado) {

                const d = responseJson.objeto;
                console.log(d);

                $("#inputGroupSubTotal").text(`Sub total - ${d.simboloMoneda}`)
                $("#inputGroupIGV").text(`IGV(${d.porcentajeImpuesto}%) - ${d.simboloMoneda}`)
                $("#inputGroupTotal").text(`Total - ${d.simboloMoneda}`)

            }

           
        })



    $("#cboBuscarProducto").select2({
        ajax: {
            url: "/Venta/ObtenerProducto",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                return {
                    busqueda: params.term
                };
            },
            processResults: function (data) {

                return {
                    results: data.map((item) => (
                    {
                       id: item.IdProducto,
                       text: item.descripcion,

                       marca: item.marca,
                       categoria: item.nombreCategoria,
                       urlImagen: item.urlImagen,
                       precio: parseFloat(item.precio)

                    }
                    ))
                };
            },
        },
        languaje: "es",
        placeholder: 'Buscar Producto...',
        minimumInputLength: 1,
        templateResult: formateResultado
    });


})


function formateResultado(data) {
    if (data.loading) {
        return data.text;
    }

    var contenedor = $(
        `<table width="100%">
            <tr>
                <td style="width:60px">
                    <img style="height:60px;width:60px;margin-right:10px" src="${data.urlImagen}"/>
                </td>
                <td>
                    <p style="font-weight: bolder;margin:2px">${data.marca}</p>
                    <p style="margin:2px">${data.text}</p>
                </td>
            </tr>
        </table>`
    );

    return contenedor;
}