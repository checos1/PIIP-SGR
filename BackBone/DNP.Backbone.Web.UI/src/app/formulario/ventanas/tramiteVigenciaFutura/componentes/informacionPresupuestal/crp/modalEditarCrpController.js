(function () {
    'use strict';

    modalEditarCrpController.$inject = [
        '$sessionStorage',
        '$uibModalInstance',
        'utilidades',
        'tramiteVigenciaFuturaServicio',
        'justificacionCambiosServicio',
        'constantesBackbone',
    ];

    function modalEditarCrpController(
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

            //document.getElementById("tituloAgregar").style.display = "none";
            //document.getElementById("tituloAgregar").style.visibility = "hidden";
            //document.getElementById("tituloEditar").style.display = "inline";
            //document.getElementById("tituloEditar").style.visibility = "visible";

            if ($uibModalInstance.esAjuste) {
                //document.getElementById("tituloModal").textContent = "Editar RP";
                vm.model.idNumero = $uibModalInstance.numeroCDPEditar;
                vm.model.idFecha = $uibModalInstance.fechaEditar;
                vm.model.idValorTotal = $uibModalInstance.valorCDPTramiteEditar;
                vm.model.idNumeroContrato = $uibModalInstance.numeroContratoEditar;
                vm.model.idValorCDPTramite =  $uibModalInstance.valorTotalEditar;
            }
            //para guardar los capitulos modificados y que se llenen las lunas
        }
        function guardar() {

            var tramiteRequisitosDto = [];
            var mostrar = false;

            if ($uibModalInstance.esAjuste) {
                tramiteRequisitosDto = $uibModalInstance.listaCRP.filter(x => x.NumeroCDP != $uibModalInstance.numeroCDPEditar);
            } else {
                tramiteRequisitosDto = $uibModalInstance.listaCRP;
            }

            if (!vm.model.idNumero) {
                document.getElementById("errorNumero").style.visibility = "visible";
                mostrar = true;
            }
            else {
                document.getElementById("errorNumero").style.visibility = "hidden";
            }

            if (!vm.model.idNumeroContrato) {
                document.getElementById("errorNumeroContrato").style.visibility = "visible";
                mostrar = true;
            }
            else {
                document.getElementById("errorNumeroContrato").style.visibility = "hidden";
            }

            if (!vm.model.idFecha) {
                document.getElementById("errorFecha").style.visibility = "visible";
                mostrar = true;
            }
            else {
                document.getElementById("errorFecha").style.visibility = "hidden";
            }

            if (!vm.model.idValorTotal) {
                document.getElementById("errorValorTotal").style.visibility = "visible";
                mostrar = true;
            }  
            else {
                document.getElementById("errorValorTotal").style.visibility = "hidden";
            }
            
            if (!vm.model.idValorCDPTramite) {
                document.getElementById("errorValorRP").style.visibility = "visible";
                mostrar = true;
            }
            else {
                document.getElementById("errorValorRP").style.visibility = "hidden";
            }

            if (mostrar) {
                utilidades.mensajeError("Revise los campos señalados y valide nuevamente.", false, "Hay datos que presentan inconsistencias.");
                return false;
            }

            if (!$uibModalInstance.esAjuste) {
                var len = vm.model.idNumero.length;
                if (len > 15) {
                    utilidades.mensajeError("Verifique el Número, máxima longitud permitida 15 caracteres");
                    return false;
                }

                var cdp = $uibModalInstance.listaCRP.filter(x => x.NumeroCDP === vm.model.idNumero);
                if (cdp.length > 0) {
                    utilidades.mensajeError("Verifique el Número, ya existe este número de CRP", false);
                    return false;
                }
            }
            else if ($uibModalInstance.numeroCDPEditar != vm.model.idNumero) {
                var len = vm.model.idNumero.length;
                if (len > 15) {
                    utilidades.mensajeError("Verifique el Número, máxima longitud permitida 15 caracteres");
                    return false;
                }
                var cdp = $uibModalInstance.listaCRP.filter(x => x.NumeroCDP === vm.model.idNumero);
                if (cdp.length > 0) {
                    utilidades.mensajeError("Verifique el Número, ya existe este número de CRP", false);
                    return false;
                }
            }

            if (!$uibModalInstance.esAjuste) {
                var len = vm.model.idNumeroContrato.length;
                if (len > 15) {
                    utilidades.mensajeError("Verifique el Número Contrato, máxima longitud permitida 15 caracteres");
                    return false;
                }

                var cdp = $uibModalInstance.listaCRP.filter(x => x.NumeroContrato === vm.model.idNumeroContrato); //PendienteValidación
                if (cdp.length > 0) {
                    utilidades.mensajeError("Verifique el Número Contrato, ya existe este número contrato", false);
                    return false;
                }
            }
            else if ($uibModalInstance.numeroContratoEditar != vm.model.idNumeroContrato) {
                var len = vm.model.idNumeroContrato.length;
                if (len > 15) {
                    utilidades.mensajeError("Verifique el Número Contrato, máxima longitud permitida 15 caracteres");
                    return false;
                }
                var cdp = $uibModalInstance.listaCRP.filter(x => x.NumeroContrato === vm.model.idNumeroContrato); //PendienteValidación
                if (cdp.length > 0) {
                    utilidades.mensajeError("Verifique el Número Contrato, ya existe este Número Contrato", false);
                    return false;
                }
            }

            if (vm.model.idValorTotal) {
                var len = vm.model.idValorTotal.toString().length;
                if (len > 16) {
                    utilidades.mensajeError("Verifique el Valor total $. excede longitud permitida");
                    return false;
                }
            }

            if (vm.model.idValorCDPTramite) {
                var len = vm.model.idValorTotal.toString().length;
                if (len > 16) {
                    utilidades.mensajeError("Verifique el valor RP $, excede longitud permitida");
                    return false;
                }
            }

            var tramiterequisito = {
                Descripcion: 'CRP para trámite VFO',
                FechaCDP: vm.model.idFecha,
                IdPresupuestoValoresCDP: 0,
                IdPresupuestoValoresAportaCDP: 0,
                IdProyectoRequisitoTramite: 0,
                IdProyectoTramite: 0,
                IdTipoRequisito: 2,
                NumeroCDP: limpiaNumero(vm.model.idNumero),
                NumeroContratoCDP: limpiaNumero(vm.model.idNumeroContrato),
                Tipo: 'CDP',
                UnidadEjecutora: '',
                ValorTotalCDP: vm.model.idValorTotal, 
                ValorCDP: vm.model.idValorCDPTramite,
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
                            utilidades.mensajeSuccess("los cambios serán reflejados en la línea de información correspondiente, dentro de la tabla ''Agregar RP''", false, false, false, "Los datos fueron actualizados con éxito.");
                        }
                        else {
                            utilidades.mensajeSuccess("Se han adicionado lineas de información en la parte inferior de la tabla ''Agregar RP''", false, false, false, "Los datos fueron cargados y guardados con éxito.");
                        }
                        $sessionStorage.close();
                       
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

        ////para guardar los capitulos modificados y que se llenen las lunas
        //function guardarCapituloModificado() {
        //    vm.seccionCapitulo = $uibModalInstance.seccionCapitulo
        //    var data = {
        //        //ProyectoId: 0,//$sessionStorage.InstanciaSeleccionada.ProyectoId,
        //        Justificacion: "",
        //        SeccionCapituloId: vm.seccionCapitulo,
        //        InstanciaId: $uibModalInstance.idInstancia,
        //        Modificado: 1,
        //        cuenta: 1
        //    }
        //    justificacionCambiosServicio.guardarCambiosFirme(data)
        //        .then(function (response) {
        //            if (response.data.Exito) {
        //                vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
        //            }
        //            else {
        //                utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
        //            }
        //        });
        //}

    }

    angular
        .module('backbone')
        .controller('modalEditarCrpController', modalEditarCrpController);

})();
