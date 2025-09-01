(function () {
    'use strict';

    angular
        .module('backbone')
        .component('simbologia', {
            templateUrl: "/src/app/acciones/consultarAcciones/componentes/simbologia.html",
            controller: function simbologiaController() {
                var varGlobales = {
                    fuente: "Work Sans",
                    tamanoFuente: 12,
                    verde: "#16B505",
                    azul:"#004884"
                };
                var lienzo = Raphael("lienzo", 195, 80);
                lienzo.circle(11, 15, 10).attr({
                    fill: varGlobales.verde, stroke: varGlobales.verde, "fill-opacity": 0.2
                });
                lienzo.text(25, 15, "Evento inicio").attr({
                    "text-anchor": "start",
                    "font-family": varGlobales.fuente
                });

                lienzo.circle(11, 40, 10).attr({
                    fill: varGlobales.azul, stroke: varGlobales.azul, "fill-opacity": 0.2
                });
                lienzo.text(26, 40, "Evento fin").attr({
                    "text-anchor": "start",
                    "font-family": varGlobales.fuente
                });

                lienzo.rect(2, 55, 20, 20, 3).attr({ stroke: varGlobales.azul });
                lienzo.text(25, 65, "Actividad").attr({
                    "text-anchor": "start",
                    "font-family": varGlobales.fuente
                });

                var diagonal = Math.sqrt(2) * 10;
                var rotate = "r-45," + 100 + "," + 15;
                lienzo.rect(100, 15, 10, 10).animate({ transform: rotate }, 0.5).attr({ stroke: "#004884" });
                lienzo.text(100 + (diagonal / 2), 15, "+").attr({
                    "text-anchor": "middle",
                    "font-size": 20,
                    stroke: "#004884"
                });
                lienzo.text(120, 15, "Flujo paralelo").attr({
                    "text-anchor": "start",
                    "font-family": varGlobales.fuente
                });

                lienzo.rect(101, 35, 10, 10);
                lienzo.text(106, 40, "+").attr({
                    "text-anchor": "middle",
                    "font-size": 20,
                    stroke: "#004884"
                });
                lienzo.text(120, 40, "Flujo anidado").attr({
                    "text-anchor": "start",
                    "font-family": varGlobales.fuente
                });

                var diagonal = Math.sqrt(2) * 10;
                var rotate = "r-45," + 100 + "," + 65;
                lienzo.rect(100, 65, 10, 10).animate({ transform: rotate }, 0.5).attr({ stroke: "#004884" });
                lienzo.text(100 + (diagonal / 2), 65, "O").attr({
                    "text-anchor": "middle",
                    "font-size": 10,
                    stroke: "#004884"
                });
                lienzo.text(120, 65, "Flujo enrutado").attr({
                    "text-anchor": "start",
                    "font-family": varGlobales.fuente
                });
            }
        });
})();