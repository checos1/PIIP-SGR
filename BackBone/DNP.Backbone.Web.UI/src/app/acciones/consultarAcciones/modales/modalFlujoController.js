(function () {
    'use strict';

    angular
        .module('backbone')
        .controller('modalFlujoController', function ($scope, $uibModalInstance, constantesAcciones, items, idAccion, nombreFlujo, info, $filter) {
            $scope.modal = $uibModalInstance;
            $scope.items = items;
            $scope.flujo = nombreFlujo;
            $scope.idAccion = idAccion;
            var conf = {
                y: 5,
                radio: 20,
                anchoTarea: 250,
                altoTarea: 180,
                anchoRama: 175,
                fuente: 'Work Sans',
                tamanoFuente12: 12,
                tamanoFuente11: 11,
                tamanoRombo: 20,
                grosorBordo1: 1,
                grosorBordo2: 2,
                flecha: 'block-wide-long',
                largoFlecha: 25,
                colorVerde: '#16B505',
                colorAzul: '#004884',
                colorGris: '#E3EAFC',
                colorBlanco: '#FFFFFF',
                colorRojo: '#F11515',
                centroX: 0,
                cursor: 'pointer',
                espacioTarea: 10,
                cinco: 5,
                diez: 10,
                quince: 15,
                veinte: 20
            };
            function dibujarDiagrama(items) {
                let anchoModal = $('#modal-diagrama > .modal-body').width();
                conf.centroX = (anchoModal / 2);
                var lienzo = Raphael("lienzo", anchoModal, 1200);
                var panZoom = lienzo.panzoom({
                    initialZoom: 0, initialPosition: { x: conf.centroX, y: conf.y }
                });
                panZoom.enable();
                conf.y = simbologia(5, conf.y, lienzo) + 50;
                var inicio = dibujarEvento(lienzo, conf.centroX, conf.y, conf.radio);
                var textoInicio = dibujarTexto(lienzo, conf.centroX, conf.y, "Inicio", conf.colorBlanco, conf.tamanoFuente11, conf.fuente, "middle");
                inicio.attr({ fill: conf.colorVerde, stroke: conf.colorVerde, "stroke-width": conf.grosorBordo1, cursor: conf.cursor });
                conf.y += conf.radio;
                var dibujarAcciones = function (acciones) {
                    for (var j = 0; j < acciones.length; j++) {
                        // acciones anidadas
                        var dibujarAccionesAnidadas = function (a) {
                            for (var k = 0; k < a.Acciones.length; k++) {
                                let subAccion = a.Acciones[k];
                                switch (subAccion.TipoAccion) {
                                    case constantesAcciones.tipo.accionTipoAnidada:
                                        conf.y -= conf.espacioTarea;
                                        lienzo.rect((conf.centroX - conf.tamanoRombo / 2), conf.y, conf.tamanoRombo, conf.tamanoRombo).attr({ stroke: conf.colorAzul });
                                        dibujarTexto(lienzo, conf.centroX, (conf.y + conf.tamanoRombo / 2), "+", conf.colorAzul, 40, conf.fontFamily, "middle");
                                        dibujarTexto(lienzo, (conf.centroX + conf.tamanoRombo / 2) + conf.espacioTarea, (conf.y + conf.tamanoRombo / 2), subAccion.Nombre, conf.colorAzul, conf.tamanoFuente12, conf.fuente);
                                        conf.y += conf.tamanoRombo;

                                        let coordSubAnidada = 'M' + conf.centroX + " " + conf.y + "L" + conf.centroX + " " + (conf.y += conf.diez);
                                        let conectorMiniSub = dibujarFlecha(lienzo, coordSubAnidada, conf.colorAzul, 1, "none");

                                        let anchoSubContenedorAnidado = conf.anchoTarea + (conf.espacioTarea * 2);
                                        let contenedorSubAnidado = dibujarTarea(lienzo, conf.centroX - anchoSubContenedorAnidado / 2, conf.y, conf.anchoTarea + (conf.espacioTarea * 2), conf.altoTarea, 8, conf.grosorBordo2, conf.colorAzul);
                                        contenedorSubAnidado.toBack();
                                        dibujarTexto(lienzo, (conf.centroX - anchoSubContenedorAnidado / 2), conf.y - conf.espacioTarea, subAccion.OrdenVisualizacion, conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                        conf.y += conf.espacioTarea;
                                        dibujarAccionesAnidadas(subAccion.FlujoAnidado);
                                        let altoSubContenedor = totalAccionesTransaccionales + (conf.espacioTarea * 2);
                                        contenedorSubAnidado.attr({ height: altoSubContenedor + conf.espacioTarea, fill: conf.colorBlanco });
                                        anchoUltimaAccion = contenedorSubAnidado.attr("width");
                                        contieneSubContenedor = true;
                                        break;
                                    case constantesAcciones.tipo.accionTipoTransaccional:
                                        let yTareaFinal = conf.y + conf.veinte;
                                        let tareaAnidadaFinal = dibujarTarea(lienzo, (conf.centroX - conf.anchoTarea / 2), conf.y, conf.anchoTarea, conf.altoTarea, 8, conf.grosorBordo1, conf.colorAzul);
                                        tareaAnidadaFinal.node.onclick = (function (k) {
                                            return function () {
                                                $scope.modal.close(subAccion);
                                                return false;
                                            }
                                        })(k);
                                        // información dentro de las acciones
                                        dibujarTexto(lienzo, (conf.centroX - conf.anchoTarea / 2) + conf.espacioTarea, yTareaFinal, reverseString(subAccion.OrdenVisualizacion), conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                        dibujarTexto(lienzo, (conf.centroX - conf.anchoTarea / 2) + conf.espacioTarea, yTareaFinal += conf.quince, subAccion.Nombre, conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                        dibujarTexto(lienzo, (conf.centroX - conf.anchoTarea / 2) + conf.espacioTarea, yTareaFinal += conf.quince, info.rol, conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                        dibujarTexto(lienzo, (conf.centroX - conf.anchoTarea / 2) + conf.espacioTarea, yTareaFinal += conf.quince, info.entidad, conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                        // fin
                                        agregarColorTareaPorEstatus(tareaAnidadaFinal, subAccion.Estado, conf);
                                        conf.y += conf.altoTarea + conf.espacioTarea;
                                        anchoUltimaAccion = conf.anchoTarea;
                                        totalAccionesTransaccionales += conf.altoTarea;
                                        break;
                                }
                            }
                        };
                        
                        // fin acciones anidadas
                        let accion = acciones[j];
                        //Datos de la acción
                        var roles = '';
                        accion.Roles.map(function (item) {
                            roles += item.NombreRol + ", ";
                        });
                        roles = roles.substring(0, roles.length - 2);
                        roles = "Roles: " + roles;
                        var nombreAccion = "Paso: " + accion.Nombre;
                        var textEntidad = "";
                        if (accion.Entidad != null) {
                            textEntidad = "Entidad: " + (accion.Entidad != null ? accion.Entidad : "");
                        }
                        var fechaFinalizacion = new Date();
                        var fechaCreacion = new Date();
                        switch (accion.Estado) {
                            case constantesAcciones.estado.ejecutada:
                                if (accion.FechaModificacion != null) {
                                    fechaFinalizacion = $filter('date')(new Date(accion.FechaModificacion), 'dd/MM/yyyy.HH:MM:ss');
                                }
                                else { fechaFinalizacion = 'NO FINALIZADA'; }
                                if (accion.FechaCreacion != null) {
                                    fechaCreacion = $filter('date')(new Date(accion.FechaCreacion), 'dd/MM/yyyy.HH:MM:ss');
                                }
                                else { fechaCreacion = 'SIN INICIAR'; }
                                break;
                            case constantesAcciones.estado.porDefinir:
                                fechaFinalizacion = 'NO FINALIZADA';
                                fechaCreacion = 'SIN INICIAR';
                                break;
                            case constantesAcciones.estado.pasoEnProgreso:
                                fechaFinalizacion = 'NO FINALIZADA';
                                if (accion.FechaCreacion != null) {
                                    fechaCreacion = $filter('date')(new Date(accion.FechaCreacion), 'dd/MM/yyyy.HH:MM:ss');
                                }
                                else { fechaCreacion = 'SIN INICIAR'; }
                                break;
                        }
                        fechaCreacion = "Fecha inicio: " + fechaCreacion;
                        fechaFinalizacion = "Fecha Fin: " + fechaFinalizacion;
                        
                        var coordenadasFlecha = "M" + conf.centroX + " " + conf.y + "L" + conf.centroX + " " + (conf.y += conf.largoFlecha);
                        dibujarFlecha(lienzo, coordenadasFlecha, conf.colorAzul, conf.grosorBordo1, conf.flecha);
                        switch (accion.TipoAccion) {
                            case constantesAcciones.tipo.accionTipoEnrutamiento:
                                //
                                let diagonalRomboEnruta = Math.sqrt(2) * conf.tamanoRombo;
                                var rotate = "r-45," + (conf.centroX - diagonalRomboEnruta) + "," + (conf.y += diagonalRomboEnruta);
                                lienzo.rect((conf.centroX - (diagonalRomboEnruta / 2) + 6), conf.y, conf.tamanoRombo, conf.tamanoRombo).attr({ stroke: conf.colorAzul }).animate({ transform: rotate }, 0.5);
                                dibujarTexto(lienzo, conf.centroX - 9, conf.y - (diagonalRomboEnruta / 2), "O", conf.colorAzul, 25, conf.fuente);
                                //conf.y += conf.tamanoRombo;
                                //
                                break;
                            case constantesAcciones.tipo.accionTipoTransaccional:
                                var tarea = dibujarTarea(lienzo, (conf.centroX - (conf.anchoTarea / 2)), conf.y, conf.anchoTarea, conf.altoTarea, 8, conf.grosorBordo1, conf.colorAzul);
                                tarea.node.onclick = (function (j) {
                                    return function () {
                                        $scope.modal.close(accion);
                                        return false;
                                    }
                                })(j);
                                agregarColorTareaPorEstatus(tarea, accion.Estado, conf);
                                let xTexto = (conf.centroX + conf.cinco) - (conf.anchoTarea / 2);
                                let yTexto = (conf.y + conf.veinte);
                                var numeroTarea = dibujarTexto(lienzo, xTexto, yTexto, accion.OrdenVisualizacion, conf.colorAzul, conf.tamanoFuente11, conf.fuente)
                                var nombreTarea = dibujarTextoLargo(lienzo, xTexto, yTexto += (conf.quince), nombreAccion, conf.colorAzul, conf.tamanoFuente11);
                                var nombreRol = dibujarTextoLargo(lienzo, xTexto, yTexto += (conf.quince * 2), roles, conf.colorAzul, conf.tamanoFuente11);
                                if (textEntidad)
                                    var nombreEntidad = dibujarTextoLargo(lienzo, xTexto, yTexto += (conf.quince * 2), textEntidad, conf.colorAzul, conf.tamanoFuente11);
                                dibujarTextoLargo(lienzo, xTexto, yTexto += (conf.quince * 2), fechaCreacion, conf.colorAzul, conf.tamanoFuente11);
                                dibujarTextoLargo(lienzo, xTexto, yTexto += (conf.quince * 2), fechaFinalizacion, conf.colorAzul, conf.tamanoFuente11);
                                conf.y += conf.altoTarea;
                                break;
                            case constantesAcciones.tipo.scopeRamas, constantesAcciones.tipo.scopeParalelas:
                                let diagonalRombo = Math.sqrt(2) * conf.tamanoRombo;
                                var rotate = "r-45," + (conf.centroX - diagonalRombo) + "," + (conf.y += diagonalRombo);
                                lienzo.rect((conf.centroX - (diagonalRombo / 2) + 6), conf.y, conf.tamanoRombo, conf.tamanoRombo).attr({ stroke: conf.colorAzul }).animate({ transform: rotate }, 0.5);
                                dibujarTexto(lienzo, conf.centroX - (diagonalRombo / 2), conf.y - (diagonalRombo / 2), "+", conf.colorAzul, 50, conf.fuente);
                                // dibujarTexto(lienzo, conf.centroX + conf.veinte, conf.y, accion.Nombre !== null ? accion.Nombre : "Nombre no definido", conf.colorAzul, conf.tamanoFuente, conf.fuente);
                                let totalAccionesParalelas = accion.AccionesParalelas.length;
                                let totalAncho = (totalAccionesParalelas * conf.anchoRama) + (conf.espacioTarea * 2) + ((totalAccionesParalelas - 1) * conf.espacioTarea);
                                var coordMini = "M" + conf.centroX + " " + conf.y + "L" + conf.centroX + " " + (conf.y += conf.diez);
                                dibujarFlecha(lienzo, coordMini, conf.colorAzul, conf.grosorBordo1, "none");
                                let xTareasHijas = conf.centroX - (totalAncho / 2);
                                let yTareasHijas = conf.y + conf.espacioTarea;
                                let totalAlto = [];
                                for (var i = 0; i < totalAccionesParalelas; i++) {
                                    var accionParalela = accion.AccionesParalelas[i];
                                    var totalAccionesFinales = accionParalela.Acciones.length;
                                    if (totalAccionesFinales > 0) {
                                        xTareasHijas += conf.espacioTarea;
                                        let rama = dibujarTarea(lienzo, xTareasHijas, yTareasHijas, conf.anchoRama, conf.altoTarea, 8, conf.grosorBordo1, conf.colorAzul);
                                        if (accionParalela.EsObligatoria) {
                                            rama.attr({ fill: conf.colorBlanco, stroke: conf.colorRojo, "stroke-dasharray": "--" })
                                        } else {
                                            rama.attr({ fill: conf.colorBlanco, stroke: conf.colorAzul, "stroke-dasharray": "--." })
                                        }
                                        let posicionXPadre = xTareasHijas + conf.espacioTarea;
                                        let posicionYPadre = yTareasHijas + conf.espacioTarea;
                                        let conectorLineaYInicial = posicionYPadre + conf.altoTarea;
                                        for (var n = 0; n < accionParalela.Acciones.length; n++) {
                                            let accionFinal = dibujarTarea(lienzo, posicionXPadre, posicionYPadre, conf.anchoTarea, conf.altoTarea, 8, conf.grosorBordo1, conf.colorAzul);
                                            if ((n + 1) < accionParalela.Acciones.length) {
                                                var coordLineaConnector = "M" + (posicionXPadre + conf.anchoTarea / 2) + " " + conectorLineaYInicial + "L" + (posicionXPadre + conf.anchoTarea / 2) + " " + (conectorLineaYInicial += conf.diez);
                                                dibujarFlecha(lienzo, coordLineaConnector, conf.colorAzul, conf.grosorBordo1, conf.flecha);
                                            }
                                            accionFinal.toFront();
                                            accionFinal.node.onclick = (function (i, n) {
                                                return function () {
                                                    $scope.modal.close(accion.AccionesParalelas[i].Acciones[n]);
                                                    return false;
                                                }
                                            })(i, n);
                                            accionFinal.hover(function () {
                                                rama.attr({ "stroke-width": conf.grosorBordo2 });
                                            }, function () {
                                                rama.attr({ "stroke-width": conf.grosorBordo1 });
                                            });
                                            agregarColorTareaPorEstatus(accionFinal, accionParalela.Acciones[n].Estado, conf);
                                            let yTextoParalelas = posicionYPadre + conf.veinte;
                                            dibujarTexto(lienzo, posicionXPadre + conf.espacioTarea, yTextoParalelas, reverseString(accionParalela.Acciones[n].OrdenVisualizacion), conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                            dibujarTextoLargo(lienzo, posicionXPadre + conf.espacioTarea, yTextoParalelas += (conf.quince), accionParalela.Acciones[n].Nombre, conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                            dibujarTextoLargo(lienzo, posicionXPadre + conf.espacioTarea, yTextoParalelas += (conf.quince * 2), info.rol, conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                            dibujarTextoLargo(lienzo, posicionXPadre + conf.espacioTarea, yTextoParalelas += (conf.quince * 2), info.entidad, conf.colorAzul, conf.tamanoFuente11, conf.fuente);
                                            posicionYPadre += conf.altoTarea + conf.espacioTarea;
                                            if ((n + 1) < accionParalela.Acciones.length) {
                                                conectorLineaYInicial += conf.altoTarea;
                                            }
                                        }
                                        let totalAltoAccionesHijas = posicionYPadre - yTareasHijas;
                                        rama.attr({ height: totalAltoAccionesHijas });
                                        totalAlto.push(totalAltoAccionesHijas);
                                        xTareasHijas += conf.anchoRama;
                                    }
                                }
                                var altoTotalAcciones = Math.max.apply(Math, totalAlto) + (conf.espacioTarea * 2);
                                dibujarTexto(lienzo, (conf.centroX - totalAncho / 2), (conf.y - conf.diez), accion.OrdenVisualizacion, conf.colorAzul, conf.tamanoFuente12, conf.fuente);
                                var contenedor = dibujarTarea(lienzo, conf.centroX - (totalAncho / 2), conf.y, totalAncho, altoTotalAcciones, conf.cinco, conf.grosorBordo2, conf.colorAzul);
                                contenedor.attr({ fill: conf.colorBlanco });
                                contenedor.toBack();
                                if ($scope.idAccion === accion.Id) {
                                    var g = contenedor.glow({ width: 15, opacity: 0.4, color: conf.colorAzul });
                                    g.show();
                                }
                                conf.y += altoTotalAcciones;
                                break;
                            case constantesAcciones.tipo.accionTipoAnidada:
                                lienzo.rect((conf.centroX - conf.tamanoRombo / 2), conf.y, conf.tamanoRombo, conf.tamanoRombo).attr({ stroke: conf.colorAzul });
                                dibujarTexto(lienzo, conf.centroX, (conf.y + conf.tamanoRombo / 2), "+", conf.colorAzul, 40, conf.fontFamily, "middle");
                                dibujarTexto(lienzo, (conf.centroX + conf.tamanoRombo / 2) + conf.espacioTarea, (conf.y + conf.tamanoRombo / 2), accion.Nombre, conf.colorAzul, conf.tamanoFuente12, conf.fuente);
                                conf.y += conf.tamanoRombo;
                                let coordMiniAnidada = 'M' + conf.centroX + " " + conf.y + "L" + conf.centroX + " " + (conf.y += conf.diez);
                                let conectorMini = dibujarFlecha(lienzo, coordMiniAnidada, conf.colorAzul, 1, "none");
                                var accionesAnidadas = accion.FlujoAnidado;
                                let posicionYAnidadas = conf.y;
                                conf.y += conf.espacioTarea;
                                var totalAccionesTransaccionales = 0;
                                var anchoUltimaAccion = 0;
                                var contieneSubContenedor = false;

                                dibujarAccionesAnidadas(accionesAnidadas);

                                let anchoTotalContenedor = anchoUltimaAccion + (conf.espacioTarea * 2); // conf.anchoTarea + (conf.espacioTarea * 2);
                                let altoTotalContenedor = conf.y - posicionYAnidadas;
                                if (contieneSubContenedor) {
                                    altoTotalContenedor += conf.espacioTarea;
                                    conf.y += conf.espacioTarea;
                                }

                                let contenedorTareasAnidadas = dibujarTarea(lienzo, conf.centroX - anchoTotalContenedor / 2, posicionYAnidadas, anchoTotalContenedor, altoTotalContenedor, 8, conf.grosorBordo2, conf.colorAzul);
                                contenedorTareasAnidadas.attr({ fill: conf.colorBlanco });
                                if ($scope.idAccion === accion.Id) {
                                    var g = contenedorTareasAnidadas.glow({ color: conf.colorAzul, width: 15 });
                                    g.show();
                                }
                                let nombreContenedor = dibujarTexto(lienzo, (conf.centroX - anchoTotalContenedor / 2), posicionYAnidadas - conf.espacioTarea, accion.OrdenVisualizacion, conf.colorAzul, conf.tamanoFuente12, conf.fuente);
                                contenedorTareasAnidadas.toBack();
                                break;
                        }
                    }
                }
                dibujarAcciones(items);
                let coordFlechaFin = 'M' + conf.centroX + " " + conf.y + "L" + conf.centroX + " " + (conf.y += conf.largoFlecha);
                let conectorFinal = dibujarFlecha(lienzo, coordFlechaFin, conf.colorAzul, 1, conf.flecha);
                var eventoFinal = dibujarEvento(lienzo, conf.centroX, (conf.y += conf.radio), conf.radio);
                dibujarTexto(lienzo, conf.centroX, conf.y, "Fin", conf.colorBlanco, conf.tamanoFuente11, conf.fuente, "middle");
                eventoFinal.attr({ cursor: conf.cursor, stroke: conf.colorAzul, "stroke-width": conf.grosorBordo2, fill: conf.colorAzul });
                //let pxSimbologia = anchoModal - 200;
                //let pySimbologia = conf.y - 150;
                //simbologia(pxSimbologia, pySimbologia, lienzo);
                var altoDiagrama = conf.y + 30;
                lienzo.setSize(anchoModal, altoDiagrama);
                lienzo.setViewBox(0, 0, anchoModal, altoDiagrama);
            }

            function AgregarTooltip(accion, conf, taskId, selectorX, selectorY, selectorAncho, selectorAlto) {
                /*Inicio tooltip*/
                var imagenTooltip = conf.urlSoloLecturaTooltip;
                var textoTooltip = "Edición disponible";

                var fechaFinalizacion = new Date();
                var fechaCreacion = new Date();
                var tienePermisos = false;
                var roles = '';
                accion.Roles.map(function (item) {
                    roles += item.NombreRol + ", ";
                    var existe = $sessionStorage.usuario.roles.find(x => x.IdRol == item.IdRol);
                    if (existe != null && existe != undefined)
                        tienePermisos = true;
                });
                roles = roles.substring(0, roles.length - 2);
                switch (accion.Estado) {
                    case constantesAcciones.estado.ejecutada:

                        if (!tienePermisos)
                            imagenTooltip = conf.urlSoloLecturaTooltip;
                        else
                            imagenTooltip = conf.urlEdicionBloqueadaTooltip;

                        textoTooltip = "Edición finalizada";
                        if (accion.FechaModificacion != null) {
                            fechaFinalizacion = $filter('date')(new Date(accion.FechaModificacion), 'dd/MM/yyyy.HH:MM:ss');
                        }
                        else { fechaFinalizacion = 'NO FINALIZADA'; }
                        if (accion.FechaCreacion != null) {
                            fechaCreacion = $filter('date')(new Date(accion.FechaCreacion), 'dd/MM/yyyy.HH:MM:ss');
                        }
                        else { fechaCreacion = 'SIN INICIAR'; }
                        break;
                    case constantesAcciones.estado.porDefinir:
                        if (!tienePermisos)
                            imagenTooltip = conf.urlSoloLecturaTooltip;
                        else
                            imagenTooltip = conf.urlEsperarEditarTooltip;

                        textoTooltip = "Edición no permitida";
                        fechaFinalizacion = 'NO FINALIZADA';
                        fechaCreacion = 'SIN INICIAR';

                        break;
                    case constantesAcciones.estado.pasoEnProgreso:
                        if (!tienePermisos) {

                            textoTooltip = "Edición no disponible";
                            imagenTooltip = conf.urlSoloLecturaTooltip;
                        }
                        else {
                            textoTooltip = "Edición disponible";
                            imagenTooltip = conf.urlEditarTooltip;
                        }

                        fechaFinalizacion = 'NO FINALIZADA';
                        if (accion.FechaCreacion != null) {
                            fechaCreacion = $filter('date')(new Date(accion.FechaCreacion), 'dd/MM/yyyy.HH:MM:ss');
                        }
                        else { fechaCreacion = 'SIN INICIAR'; }
                        break;
                    default:
                        imagenTooltip = conf.urlEditarTooltip;
                        textoTooltip = "Edición disponible";
                }

                var posiciondvx = conf.posicionX - 8;
                var posiciondvy = conf.posicionY - 15;
                var elemento = angular.element(document.getElementById('paper'));
                var usuario = "";
                var usuarioCedula = "";
                var entidad = "";
                if (accion.Entidad != null) {
                    entidad = `<p style="text-transform: capitalize; "><span>Entidad: ${accion.Entidad}</span></p>`;
                }


                if (accion.ModificadoPor != null) {
                    usuarioCedula = `<p><span>Nombre del Usuario: ${accion.NombreUsuario} - ${accion.ModificadoPor}</span></p>`;
                }

                var informacion = `<div class='tooltipInfo'>
                                        <div>
                                            <div>
                                                <div>
								                    <img style="width:16px; height:16px" src="${imagenTooltip}">
								                    <img src="${conf.urlSeparador}">
								                    <span class="titleTooltip">${textoTooltip}</span>
							                    <div>
							                    <br/>
                                                <br/>
                                                <p style="font-weight:600"><span>Paso: ${accion.Nombre}</span></p>
                                                <p><span>Roles: ${roles}</span></p>
                                                ${usuarioCedula}
                                                ${entidad}
                                                <p><span>Fecha Inicio: ${fechaCreacion}</span></p>
                                                <p><span>Fecha Fin: ${fechaFinalizacion}</span></p>
                                            </div>
                                        </div >
                                    </div >`;


                var angularHtml = angular.element(informacion);
                elemento.append(angularHtml);
                $compile(elemento)($scope);

                /*Fin tooltip */
            }

            function simbologia(x, y, papel) {
                let yInicial = y;
                papel.text((x + 395), y, "Simbología").attr({ "text-anchor": "middle", "font-size": conf.tamanoFuente12, "font-family": conf.fuente, fill: conf.colorAzul });
                papel.rect(x, y += 12, 800, 50, 8).attr({ "stroke-width": conf.grosorBordo2, stroke: conf.colorAzul });

                papel.circle(x + 35, y += 15, 10).attr({
                    fill: conf.colorVerde,
                    stroke: conf.colorVerde,
                    fill: conf.colorVerde
                });
                papel.text(x + 50, y, "Inicio").attr({
                    "text-anchor": "start",
                    "font-family": conf.fuente,
                    "font-size": conf.tamanoFuente11,
                    fill: conf.colorAzul
                });

                papel.circle(x + 400, y, 10).attr({
                    fill: conf.colorAzul,
                    stroke: conf.colorAzul
                });
                papel.text(x + 415, y, "Fin").attr({
                    "text-anchor": "start",
                    "font-family": conf.fuente,
                    "font-size": conf.tamanoFuente11,
                    fill: conf.colorAzul
                });

                papel.rect(x + 660, y - 10, 20, 20, 5).attr({ stroke: conf.colorAzul });
                papel.text(x + 685, y, "Actividad").attr({
                    "text-anchor": "start",
                    "font-family": conf.fuente,
                    "font-size": conf.tamanoFuente11,
                    fill: conf.colorAzul
                });



                //papel.text(x + 30, y += 10, "Paso en progreso").attr({
                //    "text-anchor": "start",
                //    "font-family": conf.fuente,
                //    "font-size": conf.tamanoFuente11
                //});


                //var diagonal = Math.sqrt(2) * 10;
                //var rotate = "r-45," + (x + 390) + "," + (y + (diagonal / 2));
                //papel.rect(x + 390, y, 10, 10).animate({ transform: rotate }, 0.5).attr({ stroke: conf.colorAzul });
                //papel.text((x + 385) + (diagonal / 2), y + 2, "+").attr({
                //    "text-anchor": "middle",
                //    "font-size": 20,
                //    stroke: conf.colorAzul
                //});
                //papel.text((x + 405), y, "Flujo paralelo").attr({
                //    "text-anchor": "start",
                //    "font-family": conf.fuente,
                //    "font-size": conf.tamanoFuente11,
                //    fill: conf.colorAzul
                //});

                //papel.rect((x + 520), y - 4, 10, 10);
                //papel.text((x + 525), y + 1, "+").attr({
                //    "text-anchor": "middle",
                //    "font-size": 20,
                //    stroke: conf.colorAzul
                //});
                //papel.text((x + 535), y, "Flujo anidado").attr({
                //    "text-anchor": "start",
                //    "font-family": conf.fuente,
                //    fill: conf.colorAzul
                //});

                //var rotate = "r-45," + (x + 650) + "," + (y + (diagonal / 2));
                //papel.rect(x + 652, y, 10, 10).animate({ transform: rotate }, 0.5).attr({ stroke: conf.colorAzul });
                //papel.text((x + 646) + (diagonal / 2), y, "O").attr({
                //    "text-anchor": "middle",
                //    "font-size": 10,
                //    stroke: conf.colorAzul
                //});
                //papel.text((x + 665), y, "Flujo enrutado").attr({
                //    "text-anchor": "start",
                //    "font-family": conf.fuente,
                //    fill: conf.colorAzul
                //});

                papel.rect(x + 65, y += 15, 10, 8, 3).attr({
                    stroke: conf.colorVerde,
                    fill: conf.colorVerde,
                    "fill-opacity": 0.2
                });
                papel.text(x + 80, y + 4, "Actividad finalizada").attr({
                    "text-anchor": "start",
                    "font-family": conf.fuente,
                    "font-size": 10,
                    fill: conf.colorVerde
                });

                papel.rect(x + 325, y, 10, 8, 3).attr({
                    stroke: conf.colorAzul,
                    fill: conf.colorAzul,
                    "fill-opacity": 0.2
                });
                papel.text(x + 340, y + 4, "Actividad en progreso").attr({
                    "text-anchor": "start",
                    "font-family": conf.fuente,
                    "font-size": 10,
                    fill: conf.colorAzul
                });

                papel.rect(x + 585, y, 10, 8, 3).attr({
                    stroke: conf.colorAzul,
                    fill: "none"
                });
                papel.text(x + 600, y + 4, "Actividad por definir").attr({
                    "text-anchor": "start",
                    "font-family": conf.fuente,
                    "font-size": 10,
                    fill: conf.colorAzul
                });
                return y;
            }
            function reverseString(str) {
                return str.split('.').reverse().join('.');
            }
            function agregarColorTareaPorEstatus(tarea, estatus, conf) {
                switch (estatus) {
                    case constantesAcciones.estado.ejecutada:
                        tarea.attr({
                            stroke: conf.colorVerde,
                            fill: conf.colorVerde,
                            "fill-opacity": 0.2
                        });
                        break;
                    case constantesAcciones.estado.porDefinir:
                        tarea.attr({
                            stroke: conf.colorAzul,
                            fill: conf.colorBlanco,
                            "fill-opacity": 0.2
                        });
                        break;
                    case constantesAcciones.estado.pasoEnProgreso:
                        tarea.attr({
                            stroke: conf.colorAzul,
                            fill: conf.colorAzul,
                            "fill-opacity": 0.2
                        });
                        break;
                    default:
                        tarea.attr({
                            stroke: conf.colorAzul,
                            fill: conf.colorBlanco,
                            "fill-opacity": 0.2
                        });
                }
            }

            function dibujarEvento(papel, posicionx, posiciony, radio) {
                return papel.circle(posicionx, posiciony, radio);
            }
            function dibujarFlecha(papel, coordenadas, colorLinea, grosorLinea, estiloFlecha) {
                var flecha = papel.path(coordenadas).attr({
                    stroke: colorLinea,
                    'stroke-width': grosorLinea,
                    'arrow-end': estiloFlecha
                });
                return flecha;
            }
            function dibujarTarea(papel, posicionx, posiciony, ancho, alto, bordoRedondeado, grosorLinea, color = '#000000') {
                return papel.rect(posicionx, posiciony, ancho, alto, bordoRedondeado).attr({ "stroke-width": grosorLinea, stroke: color, fill: color, "fill-opacity": 0.2, cursor: "pointer" });
            }
            function dibujarTexto(papel, posicionx, posiciony, texto, color, fontSize = 12, fontFamily = 'Work Sans', alineacionTexto = 'start') {
                var texto = papel.text(posicionx, posiciony, texto).attr({
                    'font-family': fontFamily,
                    'font-size': fontSize,
                    'fill': color,
                    'text-anchor': alineacionTexto
                });
            }

            function dibujarTextoLargo(papel, posicionx, posiciony, texto, color, fontSize = 12, fontFamily = 'Work Sans', alineacionTexto = 'start') {
                var arrayDeCadenas = texto.split(' ');
                var longitud = 35;
                var acumuladolongitud = 0;
                var cantidadveces = 1;
                var textotmp = '';
                arrayDeCadenas.map(function (item, index) {
                    acumuladolongitud += item.length;
                    var palabra = '';
                    if (acumuladolongitud > longitud) {
                        acumuladolongitud = item.length;
                        palabra += '\n';
                        cantidadveces++;
                    }
                    else if (index > 0) {
                        palabra += ' ';
                    }
                    textotmp += (palabra += item);
                });
                var texto = papel.text(posicionx, posiciony, textotmp).attr({
                    'font-family': fontFamily,
                    'font-size': fontSize,
                    'fill': color,
                    'text-anchor': alineacionTexto
                });
                return cantidadveces;
            }

            $scope.modal.rendered.then(function () {
                dibujarDiagrama($scope.items);
            });
        });
})();
