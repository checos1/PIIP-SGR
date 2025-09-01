(function () {
    'use strict';

    certificadoDisponibilidadPresupuestalSgpFormulario.$inject = [
        '$scope',
        'utilidades',
        'trasladosServicio',
        '$sessionStorage',
        'utilsValidacionSeccionCapitulosServicio'
    ];

    function certificadoDisponibilidadPresupuestalSgpFormulario(
        $scope,
        utilidades,
        trasladoServicio,
        $sessionStorage,
        utilsValidacionSeccionCapitulosServicio
    ) {
        var vm = this;

        vm.proyectosList = [];
        vm.componentesRefresh = [
        ];
        vm.proyectosListError = [];
        vm.idRol = '';
        vm.getProyectoSuccess = false;
        vm.nombreComponente = 'informacionpresupuestalsgpactoadmtramitesgp';
        vm.errorList = {};

        vm.init = function () {
            vm.setEvents();
            vm.refreshComponente();
            vm.notificacionvalidacion({ handler: vm.notificacionValidacionPadre, nombreComponente: vm.nombreComponente, esValido: true });
            vm.notificacioncambios({ handler: vm.notificacionCambiosCapitulos, nombreComponente: vm.nombreComponente });
        }
        vm.errores002 = ''
        vm.setEvents = function () {
            $scope.$watch('vm.tramiteid', function () {
                if (vm.tramiteid !== '') {
                    vm.getProyectosByTramite();
                }
            });

            $scope.$watch('vm.actualizacomponentes', function () {
                if (vm.tramiteid !== '') {
                    vm.getProyectosByTramite();
                }
            });
        }

        vm.refreshComponente = function () {
            vm.obtenerRolActual();
            if (vm.tramiteid !== undefined && vm.tramiteid !== null && vm.tramiteid !== '') {
                vm.getProyectosByTramite();
            }
        }
        vm.obtenerRolActual = function () {
            const objetoNegocioInfo = vm.obtenerSessionStorageData();

            trasladoServicio.obtenerInstanciasPermiso(objetoNegocioInfo).then(function (result) {
                const rolPorInstanciaFlujo = result.data.find(w => w.InstanciaId == objetoNegocioInfo.InstanciaId && w.FlujoId == objetoNegocioInfo.FlujoId);
                if (rolPorInstanciaFlujo) {
                    vm.idRol = rolPorInstanciaFlujo.RolId;
                }
            });
        }

        vm.obtenerSessionStorageData = function () {
            const usuarioRolesRaw = $sessionStorage.usuario.roles !== undefined ? $sessionStorage.usuario.roles : [];
            const usuarioRoles = usuarioRolesRaw.map((item) => item.IdRol);
            const entidadId = $sessionStorage.idEntidad !== undefined ? $sessionStorage.idEntidad : '';
            const idUsuarioDNP = $sessionStorage.usuario.permisos.IdUsuarioDNP !== undefined ? $sessionStorage.usuario.permisos.IdUsuarioDNP : '';
            const flujoId = $sessionStorage.idFlujoIframe;
            const instanciaId = $sessionStorage.idInstancia;

            return {
                EntidadId: entidadId,
                IdUsuarioDNP: idUsuarioDNP,
                IdsRoles: usuarioRoles,
                InstanciaId: instanciaId,
                FlujoId: flujoId,
                UsuarioDNP: ''
            }
        }


        vm.getProyectosByTramite = function () {
            vm.getProyectoSuccess = false;
            trasladoServicio.obtenerProyectosTramite(vm.tramiteid)
                .then(function (response) {
                    vm.getProyectoSuccess = true;
                    if (response.status === 200) {
                        vm.proyectosList = response.data !== undefined && response.data !== null ?
                            response.data.filter(w => w.TipoProyecto.toLowerCase() === 'contracredito') :
                            [];
                        vm.mapearNombreProyectos();
                    }
                }, function (error) {
                    vm.getProyectoSuccess = true;
                    utilidades.mensajeError('No fue posible consultar el listado de proyectos asociados al trámite');
                });
        }

        vm.mapearNombreProyectos = function () {
            vm.proyectosList = vm.proyectosList.map(
                (proyecto) => {
                    let nombreProyectoCorto = proyecto.NombreProyecto;
                    let nombreCompleto = proyecto.NombreProyecto;
                    if (proyecto.NombreProyecto !== undefined && proyecto.NombreProyecto.length > 80) {
                        nombreProyectoCorto = proyecto.NombreProyecto.substring(0, 80);
                    }

                    return {
                        ...proyecto,
                        NombreProyecto: nombreProyectoCorto,
                        NombreProyectoCompleto: nombreCompleto
                    }
                }
            );
        }


        vm.expandirProyecto = function (proyectoId) {
            const verMas = document.getElementById('verMas-' + proyectoId);

            if (window.getComputedStyle(verMas).display == 'none') {
                verMas.style.display = 'block';
                document.getElementById('verMenos-' + proyectoId).style.display = 'none'
            } else {
                verMas.style.display = 'none';
                document.getElementById('verMenos-' + proyectoId).style.display = 'block'
            }
        }

        vm.notificacionCambiosCapitulos = function (nombreCapituloHijo) {
            console.log("vm.notificacionCambiosCapitulos acto administrativo ", nombreCapituloHijo)
            if (vm.componentesRefresh.includes(nombreCapituloHijo)) {
                vm.refreshComponente();
            }
        }

        vm.verNombreCompleto = function (idVerMas, idProyecto) {
            if (document.getElementById(idVerMas).classList.contains("proyecto-nombre")) {
                document.getElementById(idVerMas).classList.remove("proyecto-nombre");
                document.getElementById(idVerMas).classList.add("proyecto-nombre-completo");
                document.getElementById(idVerMas).innerText = vm.proyectosList.find(w => w.ProyectoId == idProyecto).NombreProyectoCompleto;
                document.getElementById("btnVerMasNombre-" + idProyecto).innerText = "Ver menos"
            } else {
                document.getElementById(idVerMas).classList.remove("proyecto-nombre-completo");
                document.getElementById(idVerMas).classList.add("proyecto-nombre");
                document.getElementById(idVerMas).innerText = vm.proyectosList.find(w => w.ProyectoId == idProyecto).NombreProyecto;
                document.getElementById("btnVerMasNombre-" + idProyecto).innerText = "Ver mas"
            }
        }

        vm.guardado = function (nombreComponenteHijo) {
            vm.guardadoevent({ nombreComponenteHijo: nombreComponenteHijo });

        }


        /* ------------------------ Validaciones ---------------------------------*/
        vm.notificacionValidacionPadre = function (errores) {
            //console.log("Validación  - CD Pvigencias futuras");
            vm.limpiarErrores(errores);
            if (errores != undefined) {
                var erroresRelacionconlapl = errores.find(p => (p.Seccion + p.Capitulo) == vm.nombreComponente);
                if (erroresRelacionconlapl != undefined) {
                    var erroresJson = erroresRelacionconlapl.Errores == "" ? [] : JSON.parse(erroresRelacionconlapl.Errores);
                    var isValid = (erroresJson == null || erroresJson.length == 0);
                    if (!isValid) {
                        erroresJson[vm.nombreComponente].forEach(p => {

                            if (vm.errores[p.Error] != undefined) vm.errores[p.Error](p.Descripcion);
                        });
                    }

                    vm.notificacionestado({ nombreComponente: vm.nombreComponente, esValido: isValid });
                }
            }
        }


        vm.limpiarErrores = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("ICDP001 - error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("ICDP002-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }

            var campoObligatorioJustificacion = document.getElementById("ICDP003-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "";
                campoObligatorioJustificacion.classList.add('hidden');
            }


            if (vm.proyectosList !== undefined) {
                vm.proyectosList.map(function (proyecto) {
                    //Busca los proyectos y borra los errores
                    var campoObligatorioJustificacion = document.getElementById("ICDP003-" + proyecto.ProyectoId);
                    if (campoObligatorioJustificacion != undefined) {
                        campoObligatorioJustificacion.innerHTML = "";
                        campoObligatorioJustificacion.classList.add('hidden');
                    }

                });
            }

        }

        vm.validarValoresICDP001 = function (index1, index2, errores) {
            var campoObligatorioJustificacion = document.getElementById('ICDP001 - error');
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span>" + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresICDP001Grilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("ErrorGrilla-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresICDP002 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("ICDP002-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span> " + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresICDP003 = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("ICDP003-error");
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span> " + errores + "</span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.validarValoresICDP003Grilla = function (errores) {
            var campoObligatorioJustificacion = document.getElementById("ICDP003-" + errores);
            if (campoObligatorioJustificacion != undefined) {
                campoObligatorioJustificacion.innerHTML = "<span class='d-inline-block ico-advertencia'></span><span></span>";
                campoObligatorioJustificacion.classList.remove('hidden');
            }
        }

        vm.errores = {
            'ICDP001': vm.validarValoresICDP001,
            'ICDP001-': vm.validarValoresICDP001Grilla,
            'ICDP002': vm.validarValoresICDP002,
            'ICDP003': vm.validarValoresICDP003,
            'ICDP003-': vm.validarValoresICDP003Grilla,
        }
    }

    angular.module('backbone').component('certificadoDisponibilidadPresupuestalSgpFormulario', {
        templateUrl: "src/app/formulario/ventanas/comun/certificadoDisponibilidadPresupuestalSgp/componentes/certificadoDisponibilidadPresupuestalSgpFormulario.html",
        controller: certificadoDisponibilidadPresupuestalSgpFormulario,
        controllerAs: "vm",
        bindings: {
            tramiteid: '@',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificacioncambios: '&',
            guardadocomponent: '&',
            guardadoevent: '&',
            actualizacomponentes: '@',
            callback: '&',
        }
    });
})();