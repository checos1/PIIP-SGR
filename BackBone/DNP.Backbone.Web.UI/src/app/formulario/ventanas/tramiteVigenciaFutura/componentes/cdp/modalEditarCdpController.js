(function () {
    'use strict';

    modalEditarCdpController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'utilidades',
        'tramiteVigenciaFuturaServicio',
        'justificacionCambiosServicio',
        'constantesBackbone',
    ];

    function modalEditarCdpController(
        $uibModalInstance,
        $sessionStorage,
        utilidades,
        tramiteVigenciaFuturaServicio,
        justificacionCambiosServicio,
        constantesBackbone,
    ) {
        var vm = this;
        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = $sessionStorage.close;
        vm.ProyectoId = $uibModalInstance.proyectoId;
        vm.TramiteId = $uibModalInstance.tramiteId;
        vm.TipoRolId = 0;
        vm.IdInstancia = $uibModalInstance.idInstancia;
        vm.idFlujo = $uibModalInstance.idFlujoIframe;
        vm.idRol = '';
        vm.idsroles = [];
        vm.parametrosObjetosNegocioDto = {};

        //para guardar los capitulos modificados y que se llenen las lunas
        vm.seccionCapitulo = null;

        function init() {
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }

            vm.parametrosObjetosNegocioDto.EntidadId = $uibModalInstance.idEntidad;
            vm.parametrosObjetosNegocioDto.IdUsuarioDNP = $uibModalInstance.usuario.permisos.IdUsuarioDNP;
            vm.parametrosObjetosNegocioDto.UsuarioDNP = '';
            $uibModalInstance.usuario.roles.map(function (item) {
                vm.idsroles.push(item.IdRol);
            });
            vm.parametrosObjetosNegocioDto.IdsRoles = vm.idsroles;
            tramiteVigenciaFuturaServicio.obtenerInstanciasPermiso(vm.parametrosObjetosNegocioDto).then(function (result) {
                result.data.map(function (item) {
                    if (item.InstanciaId === vm.IdInstancia && item.FlujoId === vm.idFlujo) {
                        vm.idsroles.map(function (itemRol) {
                            if (itemRol === item.RolId)
                                vm.idRol = item.RolId;
                        });
                    }
                });
            });

            if ($uibModalInstance.esAjuste) {
                vm.model.idNumero = $uibModalInstance.numeroCDPEditar;
                vm.model.idFecha = $uibModalInstance.fechaEditar;
                vm.model.idValorTotal = $uibModalInstance.valorTotalEditar;
                vm.model.idValorCDPTramite = $uibModalInstance.valorCDPTramiteEditar;
            }
            //para guardar los capitulos modificados y que se llenen las lunas

        }
        function guardar() {

            var tramiteRequisitosDto = [];

            if ($uibModalInstance.esAjuste) {
                tramiteRequisitosDto = $uibModalInstance.listaCDP.filter(x => x.NumeroCDP != $uibModalInstance.numeroCDPEditar);
            } else {
                tramiteRequisitosDto = $uibModalInstance.listaCDP;
            }

            if (!vm.model.idNumero) {
                utilidades.mensajeError("Verifique el Número.", false); return false;
            }
            else if (!$uibModalInstance.esAjuste) {
                var len = vm.model.idNumero.length;
                if (len > 15) {
                    utilidades.mensajeError("Verifique el Número, máxima longitud permitida 15 caracteres"); return false;
                }

                var cdp = $uibModalInstance.listaCDP.filter(x => x.NumeroCDP === vm.model.idNumero);
                if (cdp.length > 0) {
                    utilidades.mensajeError("Verifique el Número, ya existe este número de CDP", false); return false;
                }
            }
            else if ($uibModalInstance.numeroCDPEditar != vm.model.idNumero) {
                var len = vm.model.idNumero.length;
                if (len > 15) {
                    utilidades.mensajeError("Verifique el Número, máxima longitud permitida 15 caracteres"); return false;
                }
                var cdp = $uibModalInstance.listaCDP.filter(x => x.numeroCDP === vm.model.idNumero);
                if (cdp.length > 0) {
                    utilidades.mensajeError("Verifique el Número, ya existe este número de CDP", false); return false;
                }
            }

            if (!vm.model.idFecha) {
                utilidades.mensajeError("Verifique la fecha.", false); return false;
            }

            if (!vm.model.idValorTotal) {
                utilidades.mensajeError("Verifique el valor CDP.", false); return false;
            }
            else {
                var len = vm.model.idValorTotal.toString().length;
                if (len > 16) {
                    utilidades.mensajeError("Verifique el valor CDP. excede longitud permitida"); return false;
                }
            }

            if (!vm.model.idValorCDPTramite) {
                utilidades.mensajeError("Verifique el valor CDP para trámite.", false); return false;
            }
            else {
                var len = vm.model.idValorTotal.toString().length;
                if (len > 16) {
                    utilidades.mensajeError("Verifique el valor CDP para trámite, excede longitud permitida"); return false;
                }
            }

            var tramiterequisito = {
                Descripcion: 'CDP para trámite VFO',
                FechaCDP: vm.model.idFecha,
                IdPresupuestoValoresCDP: 0,
                IdPresupuestoValoresAportaCDP: 0,
                IdProyectoRequisitoTramite: 0,
                IdProyectoTramite: 0,
                IdTipoRequisito: 1,
                NumeroCDP: limpiaNumero(vm.model.idNumero),
                Tipo: 'CDP',
                UnidadEjecutora: '',
                ValorCDP: vm.model.idValorTotal,
                ValorTotalCDP: vm.model.idValorCDPTramite,
                IdValorTotalCDP: 0,
                IdValorAportaCDP: 0,
                IdProyecto: vm.ProyectoId,
                IdTramite: vm.TramiteId,
                IdTipoRol: vm.TipoRolId,
                IdRol: vm.idRol
            }
            tramiteRequisitosDto.push(tramiterequisito);

            tramiteRequisitosDto = tramiteRequisitosDto.map(a => {
                a.ValorCDP = a.ValorCDP.toString().replaceAll(',', '.');
                a.ValorTotalCDP = a.ValorTotalCDP.toString().replaceAll(',', '.');
                return a;
            });


            tramiteVigenciaFuturaServicio.actualizarTramitesRequisitos(tramiteRequisitosDto)
                .then(function (response) {
                    let exito = response.data;
                    if (exito) {
                        if ($uibModalInstance.esAjuste) {
                            utilidades.mensajeSuccess("los cambios serán reflejados en la línea de información correspondiente, dentro de la tabla ''Agregar CDP''", false, false, false, "Los datos fueron actualizados con éxito.");
                        }
                        else {
                            utilidades.mensajeSuccess("Se han adicionado lineas de información en la parte inferior de la tabla ''Agregar CDP''", false, false, false, "Los datos fueron guardados con éxito.");
                        }
                        $sessionStorage.close();
                        //para guardar los capitulos modificados y que se llenen las lunas
                        guardarCapituloModificado();
                    }
                    else {
                        utilidades.mensajeError("Error al realizar la operación", false);
                    }
                })
                .catch(error => {
                    if (error.status == 400) {
                        utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                        return;
                    }
                    utilidades.mensajeError("Error al realizar la operación");
                });
        }
        function limpiaNumero(valor) {
            return valor.toString().replaceAll(".", "");
        }

        //para guardar los capitulos modificados y que se llenen las lunas
        function guardarCapituloModificado() {
            vm.seccionCapitulo = $uibModalInstance.seccionCapitulo
            var data = {
                //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo,
                InstanciaId: $uibModalInstance.idInstancia,
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

    }

    angular
        .module('backbone')
        .controller('modalEditarCdpController', modalEditarCdpController);





})();
