console.log("golaaaaaaa")

$(document).ready(function () {

    $("div.container-fluid").LoadingOverlay("show");

    fetch("/DashBoard/ObtenerResumen")
        .then(response => {
            $("div.container-fluid").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {

            if (responseJson.estado) {


                //MOSTRAR DATOS DE LAS TARJETAS
                let d = responseJson.objeto

                $("#totalVenta").text(d.totalVentas)
                $("#totalIngresos").text(d.totalIngresos)
                $("#totalProductos").text(d.totalProductos)
                $("#totalCategorias").text(d.totalCategorias)

                //OBETENER TEXTOS Y VALORES

                let barchart_labaels;
                let barchart_data;

                if (d.ventasUltimaSemanas.length > 0) {

                    barchart_labaels = d.ventasUltimaSemanas.map((item) => {return item.fecha })
                    barchart_data = d.ventasUltimaSemanas.map((item) => {return item.total })
                } else {
                    barchart_labaels = ["Sin resultados"]
                    barchart_data = [0]
                }

                //OBETENER TEXTOS Y VALORES para el grafico de pie
                let piechart_labaels;
                let piechart_data;
                if (d.productosTopUltimaSemana.length > 0) {

                    piechart_labaels = d.productosTopUltimaSemana.map((item) => { return item.producto })
                    piechart_data = d.productosTopUltimaSemana.map((item) => { return item.cantidad })
                } else {
                    piechart_labaels = ["Sin resultados"]
                    piechart_data = [0]
                }


                // Bar Chart Example
                let controlVenta = document.getElementById("chartVentas");
                let myBarChart = new Chart(controlVenta, {
                    type: 'bar', //tipo grafico
                    data: {
                        labels: barchart_labaels,
                        datasets: [{
                            label: "Cantidad",
                            backgroundColor: "#4e73df",
                            hoverBackgroundColor: "#2e59d9",
                            borderColor: "#4e73df",
                            data: barchart_data,
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        scales: {
                            xAxes: [{
                                gridLines: {
                                    display: false,
                                    drawBorder: false
                                },
                                maxBarThickness: 50,
                            }],
                            yAxes: [{
                                ticks: {
                                    min: 0,
                                    maxTicksLimit: 5
                                }
                            }],
                        },
                    }
                });



                // Pie Chart Example
                let controlProducto = document.getElementById("chartProductos");
                let myPieChart = new Chart(controlProducto, {
                    type: 'doughnut', //dona
                    data: {
                        labels: piechart_labaels,
                        datasets: [{
                            data: piechart_data,
                            backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', "#FF785B"],
                            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', "#FF5733"],
                            hoverBorderColor: "rgba(234, 236, 244, 1)",
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            backgroundColor: "rgb(255,255,255)",
                            bodyFontColor: "#858796",
                            borderColor: '#dddfeb',
                            borderWidth: 1,
                            xPadding: 15,
                            yPadding: 15,
                            displayColors: false,
                            caretPadding: 10,
                        },
                        legend: {
                            display: true
                        },
                        cutoutPercentage: 80,
                    },
                });



            }
        })
})