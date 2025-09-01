(function () {
    'use strict';

    asociarMuchosProyectosSgpFormulario.$inject = [
        '$sessionStorage',
        'utilidades',
        '$scope',
        'constantesBackbone',
        'comunesServicio',
        'justificacionCambiosServicio',
        'tramiteSGPServicio'
    ];

    function asociarMuchosProyectosSgpFormulario(
        $sessionStorage,
        utilidades,
        $scope,
        constantesBackbone,
        comunesServicio,
        justificacionCambiosServicio,
        tramiteSGPServicio
    ) {

        var vm = this;
        vm.lang = "es";
        vm.actualizadetalle = 0;
        vm.instanciaId = $sessionStorage.idInstanciaIframe;
        vm.idFlujo = $sessionStorage.idFlujoIframe;
        vm.textoBuscar = '';
        vm.checkSeleccionar = false;
        vm.estadoTramite = '';
        vm.estadoAjusteCreado = false;
        vm.proyectoAsociado = false;
        vm.proyectoId = 0;
        vm.datosproyecto = {};
        vm.listavalores = [];
        vm.seleccionProyectos = false;
        $sessionStorage.listadosubprogramas = [];
        $sessionStorage.listadoprogramas = [];
        vm.listaFiltroEntidades = [];
        vm.listaFiltroEntidadesD = [];
        vm.habilitaBotones = $sessionStorage.nombreAccion.includes($sessionStorage.listadoAccionesTramite[0].Nombre) && !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1
        vm.cabezaSector = false;
        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;
        //Para mostar los botones en el paso 3

        //Valida que este en el paso 3 para mostrar los botones
        vm.mostrar = $sessionStorage.idNivel.toUpperCase() === constantesBackbone.idNivelElaboracionConcepto
        vm.conceptoHabilitado = false;
        vm.valorHabilitado = false;
        vm.unsolotipooperacion = false;
        vm.tituloMensaje = "";
        vm.mensaje = ""

        $scope.$watch('vm.nombrecomponentepaso', function () {
            if (vm.nombrecomponentepaso !== undefined && vm.nombrecomponentepaso !== '') {
                vm.nombreComponente = vm.nombrecomponentepaso;
                vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });

            }
        });

        vm.init = function () {
            limpiarCombos();
            if (vm.proyectoId != 0)
                $sessionStorage.EstadoAsociacionVF = 'Con Asociación';
            else
                $sessionStorage.EstadoAsociacionVF = 'Sin Asociación';
            $sessionStorage.EstadoAjusteCreado = false;
            comunesServicio.obtenerEntidadTramite($sessionStorage.idObjetoNegocio).then(
                function (rta) {
                    vm.cabezaSector = rta.data[0].CabezaSector;
                    var a = a;
                }
            );

            $scope.$watch(() => vm.tramiteid
                , (newVal, oldVal) => {
                    if (newVal) {
                        if (vm.tramiteid != '') {
                            obtenerTramite();
                            if (vm.tipotramiteid != 1)
                                obtenerTramiteD();
                        }
                        vm.actualizadetalle++;
                    }
                }, true);

            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
        }

        function obtenerTramite() {

            comunesServicio.obtenerEntidadAsociarProyecto(vm.instanciaId, 'C').then(
                function (respuesta) {
                    vm.listaFiltroEntidades = respuesta.data;
                    if (vm.tipotramiteid == 1) {//trasl ordinario
                        vm.listaFiltroEntidadesD = vm.listaFiltroEntidades;
                        vm.seleccionProyectos = true;//deshabilita el combo de entidad credito
                    }
                });

        }
        function obtenerTramiteD() {
            vm.seleccionProyectos = false;
            comunesServicio.obtenerEntidadAsociarProyecto(vm.instanciaId, 'D').then(
                function (respuesta) {
                    vm.listaFiltroEntidadesD = respuesta.data;
                }
            );
        }

        $scope.$watch('vm.listaFiltroEntidades', function () {
            vm.listaProyectosC = [];
            vm.ProyectosSeleccionadosC = "";
        });

        function ObtenerProyectos(idEntidad, tipoProyecto) {
            if (idEntidad != null) {
                let listaProyectosGrid = [];
                var prm = {
                    idFlujo: vm.idFlujo,
                    tipoEntidad: 'a',
                    IdEntidad: idEntidad, idInstancia: vm.instanciaId
                };

                if (tipoProyecto == 'C') {
                    tramiteSGPServicio.obtenerContraCreditosSgp(prm)
                        .then(function (response) {                            
                            if (response.data !== null && response.data.length > 0) {
                                vm.listaProyectosC = response.data;
                            }

                            if (vm.idFlujo == '05e34608-c6ae-4146-b3e3-4e7b695b5a00') {
                                var filtro = vm.listaProyectosC.filter(function (filtro) {
                                    return filtro.MarcaTraslado === 'S';
                                });
                                vm.listaProyectosC = filtro;

                            }
                        });
                    return vm.listaProyectosC;
                }

                if (tipoProyecto == 'D') {
                    vm.listaProyectosD = [];
                    tramiteSGPServicio.obtenerCreditosSgp(prm)
                        .then(function (response) {
                            if (response.data !== null && response.data.length > 0) {
                                vm.listaProyectosD = response.data;
                            }
                        });
                    return vm.listaProyectosD;
                }
            }
        }

        vm.asociarProyecto = function () {
            let proyectos = [];
            var paso = true;
            if (vm.ProyectosSeleccionadosC && vm.ProyectosSeleccionadosC.length > 0) {
                vm.ProyectosSeleccionadosC.forEach(proyecto => {
                    var datoproyectoC = vm.listaProyectosC.filter(function (datoproyectoC) {
                        return datoproyectoC.IdProyecto === proyecto;
                    });
                    datoproyectoC.forEach(p => {
                        let c = {
                            ProyectoId: p.IdProyecto,
                            EntidadId: p.IdEntidad,
                            TipoProyecto: 'Contracredito',
                            NombreProyecto: p.NombreProyecto
                        };
                        if (vm.tipotramiteid == 8) { // solo para tramite de distribucion, solo se permite un proyecto contracredito
                            if (!proyectos.find(ent => ent.TipoProyecto === 'Contracredito') && !$sessionStorage.existecontradistribucion) {
                                proyectos.push(c);
                            }
                            else {
                                utilidades.mensajeError("La distribución previo concepto, solo permite asociar un proyecto contracrédito", false, '', '');
                                paso = false;
                            }
                        }
                        else {
                            proyectos.push(c);
                        }
                    });
                });
            }

            /// como no tiene proyectos asociados en traslado ordinario, agregamos a la lista de subprogramas los seleccionados
            if (vm.tipotramiteid == 1) {
                proyectos.forEach(p => {
                    var subprograma = p.NombreProyecto.split('Subprograma: ')[1];
                    if (!$sessionStorage.listadosubprogramas.find(ent => ent.id === subprograma)) {
                        var subprogramaAdd = { id: subprograma };
                        $sessionStorage.listadosubprogramas.push(subprogramaAdd);
                    }
                });
            }
            else if (vm.tipotramiteid == 2 && !vm.cabezaSector) {
                $sessionStorage.listadoprogramasC = [];
                $sessionStorage.listadosubprogramasC = [];
                proyectos.forEach(p => {
                    var programa = p.NombreProyecto.split('Programa: ')[1].substring(0, 4);
                    if (!$sessionStorage.listadoprogramasC.find(ent => ent.id === programa)) {
                        var programaAdd = { id: programa };
                        $sessionStorage.listadoprogramasC.push(programaAdd);
                    }
                    var subprograma = p.NombreProyecto.split('Subprograma: ')[1];
                    if (!$sessionStorage.listadosubprogramasC.find(ent => ent.id === subprograma)) {
                        var subprogramaAdd = { id: subprograma };
                        $sessionStorage.listadosubprogramasC.push(subprogramaAdd);
                    }
                });
            }

            if (vm.ProyectosSeleccionadosD && vm.ProyectosSeleccionadosD.length > 0) {
                vm.ProyectosSeleccionadosD.forEach(proyecto => {
                    var datoproyectoD = vm.listaProyectosD.filter(function (datoproyectoD) {
                        return datoproyectoD.IdProyecto === proyecto;
                    });
                    datoproyectoD.forEach(p => {
                        let c = {
                            ProyectoId: p.IdProyecto,
                            EntidadId: p.IdEntidad,
                            TipoProyecto: 'Credito',
                            NombreProyecto: p.NombreProyecto
                        };
                        if (vm.tipotramiteid == 1) { // solo para tramite de traslado ordinario
                            if ($sessionStorage.listadosubprogramas.length == 0) {
                                proyectos.push(c);
                            }
                            else {
                                var subprograma = c.NombreProyecto.split('Subprograma: ')[1];
                                if ($sessionStorage.listadosubprogramas.find(ent => ent.id === subprograma)) {
                                    proyectos.push(c);
                                }
                                else {
                                    utilidades.mensajeError("El subprograma de los proyectos deben ser los mismos para contracrédito y crédito", false, '', '');
                                }
                            }
                        }
                        else if (vm.tipotramiteid == 2 && !vm.cabezaSector) { // solo para tramite de ley
                            if ($sessionStorage.listadosubprogramas.length == 0) {
                                proyectos.push(c);
                            }
                            else {
                                var subprograma = c.NombreProyecto.split('Subprograma: ')[1];
                                var programa = c.NombreProyecto.split('Programa: ')[1].substring(0, 4);
                                if (!$sessionStorage.listadosubprogramasC.find(ent => ent.id === subprograma)
                                    || !$sessionStorage.listadoprogramasC.find(ent => ent.id === programa)) {
                                    proyectos.push(c);
                                }
                                else {
                                    utilidades.mensajeError("La clasificacion presupuestal de los proyectos no deben ser los mismos para contracrédito y crédito", false, '', '');
                                    paso = false;
                                }
                            }
                        }
                        else {
                            proyectos.push(c);
                        }
                    });
                });
            }

            if (proyectos.length > 0) {

                var prm = {
                    TramiteId: vm.tramiteid,
                    Proyectos: proyectos
                };
                if (paso) {
                    comunesServicio.guardarProyectos(prm)
                        .then(function (response) {
                            if (response.data && response.status == 200) {

                                if (response.data.Exito) {
                                    parent.postMessage("cerrarModal", window.location.origin);
                                    utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                                    limpiarCombos();
                                    vm.actualizadetalle++;
                                    guardarCapituloModificado();

                                } else {
                                    swal('', response.data.Mensaje, 'warning');
                                }
                            } else {
                                swal('', "Error al realizar la operación", 'error');
                            }
                        });
                }
            }
        }

        vm.actualizaCombos = function () {
            if (vm.tipotramiteid == 1) {
                vm.IdEntidadSeleccionadaD = vm.IdEntidadSeleccionadaCC;
                vm.actualizaCombosD();
            }

            ObtenerProyectos(vm.IdEntidadSeleccionadaCC, 'C');
        }

        vm.actualizaCombosD = function () {
            //if (vm.tipotramiteid != 1) {                    
            ObtenerProyectos(vm.IdEntidadSeleccionadaD, 'D');

        }

        vm.actualizaListaC = function () {
            if (vm.ProyectosSeleccionadosC && vm.ProyectosSeleccionadosC.length > 0) {
                vm.ProyectosSeleccionadosC.forEach(proyecto => {
                    if (!$sessionStorage.listadosubprogramas.find(ent => ent.id === proyecto.SubPrograma)) {
                        var subprograma = { id: proyectoentidad.SubPrograma };
                        $sessionStorage.listadosubprogramas.push(subprograma);
                    }
                });
            }
        }

        vm.actualizaListaD = function () {
            if (vm.ProyectosSeleccionadosD && vm.ProyectosSeleccionadosD.length > 0) {
                vm.ProyectosSeleccionadosD.forEach(proyecto => {
                    if (!$sessionStorage.listadosubprogramas.find(ent => ent.id === proyecto.SubPrograma)) {
                        var subprograma = { id: proyectoentidad.SubPrograma };
                        $sessionStorage.listadosubprogramas.push(subprograma);
                    }
                });
            }
        }

        function limpiarCombos() {
            vm.IdEntidadSeleccionadaD = "";
            vm.IdEntidadSeleccionadaCC = "";
            vm.ProyectosSeleccionadosC = "";
            vm.ProyectosSeleccionadosD = "";
        }

        vm.changeArrow = function (nombreModificado) { }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {

            ObtenerSeccionCapitulo();
            var data = {
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $sessionStorage.idInstancia,
                Modificado: 1,
                cuenta: 1
            }
            justificacionCambiosServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.guardadoevent({ nombreComponenteHijo: nombreComponenteHijo });

        }

        /* ------------------------ Validaciones ---------------------------------*/

        vm.notificacionValidacionPadre = function (errores) {

            vm.limpiarErrores(errores);
            var isValid = true;
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    if (erroresJson != undefined) {
                        isValid = (erroresJson == null || erroresJson.length == 0);
                        if (!isValid) {
                            erroresJson[vm.nombreComponente].forEach(p => {

                                if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                            });
                        }
                    }
                }
                vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
            }
        }

        vm.validarProyectoContracredito = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoContracredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarProyectoCredito = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoCredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarNoCreditoContracredito = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-NoCreditoContracredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValorCreditoContracredito = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ValorCreditoContracredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarProgramaSubprogramaCreditoContracredito = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProgramaSubprogramaCreditoContracredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarEstadoControlPosterior = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-EstadoControlPosterior-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarEstadoControlPosteriorGrilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("AS006-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.ValidaTotalAprobado = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ValidaTotalAprobado-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.ValidaMontosDiligenciados = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-montoDiligenciados-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'AS001': vm.validarProyectoContracredito,
            'AS002': vm.validarProyectoCredito,
            'AS003': vm.validarNoCreditoContracredito,
            'AS004': vm.validarValorCreditoContracredito,
            'AS005': vm.validarProgramaSubprogramaCreditoContracredito,
            'AS006': vm.validarEstadoControlPosterior,
            'AS006-': vm.validarEstadoControlPosteriorGrilla,
            'AS007': vm.ValidaTotalAprobado,
            'AS008-': vm.validarEstadoControlPosteriorGrilla,
            'AS008': vm.ValidaMontosDiligenciados,


        }

        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoContracredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProyectoCredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-NoCreditoContracredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ValorCreditoContracredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ProgramaSubprogramaCreditoContracredito-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }


            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-EstadoControlPosterior-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }


            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-ValidaTotalAprobado-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            //Esto se hace para borrar los iconos en la grilla.  En el html en la clase puse el nombre erroras006 para que solo me traiga los asociados a este error
            let elementos = document.getElementsByClassName("erroras006");
            if (elementos !== undefined) {
                let i;
                for (i = 0; i < elementos.length; i++) {

                    var campoObligatorioJustificacion = elementos[i];
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.innerHTML = "";
                        campoObligatorioJustificacion.classList.add('hidden');
                    }
                }
            }

            var campoObligatorioJustificacion = document.getElementById("detalleMuchosProyectos-montoDiligenciados-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

        }
        /* ------------------------ FINAL Validaciones ---------------------------------*/
    }

    angular.module('backbone').component('asociarMuchosProyectosSgpFormulario', {
        templateUrl: "src/app/formulario/ventanas/comun/asociarMuchosProyectos/asociarMuchosProyectosSgpFormulario.html",
        controller: asociarMuchosProyectosSgpFormulario,
        controllerAs: "vm",
        bindings: {
            callback: '&',
            guardadoevent: '&',
            deshabilitarBotonDevolverSeccionProyecto: '&',
            tramiteid: '@',
            tipotramiteid: '@',
            nombrecomponentepaso: '@',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            actualizacomponentes: '=',
            deshabilitar: '@',
            rolanalista: '@',

        }
    });

})();
