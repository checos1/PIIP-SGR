(function () {
    'use strict';

    modalDatosAdicionDonacionController.$inject = [
        '$sessionStorage', '$uibModalInstance', 'datosAdicionDonacionServicio', 'utilidades', 'constantesBackbone', '$scope'
    ];

    function modalDatosAdicionDonacionController(
        $sessionStorage, $uibModalInstance, datosAdicionDonacionServicio, utilidades, constantesBackbone, $scope
    ) {

        var vm = this;
        vm.init = init;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.cambioSector = cambioSector;
        vm.guardar = guardar;
        vm.cerrar = $uibModalInstance.close;
        vm.seccionCapitulo = null;
        vm.nombreComponente = "proyectodatosadicionpordonacion";
        vm.btnGuardarEditar = 'GUARDAR';
        vm.validateFormat = validateFormat;
        vm.convenioId = $sessionStorage.convenioId;
        vm.listaDatosAdicion = $sessionStorage.listaDatosAdicion
        vm.listaSectores = [];

        var lstRolesTodo = $sessionStorage.usuario.roles;
        var lsRoles = [];
        for (var ls = 0; ls < lstRolesTodo.length; ls++)
            lsRoles.push(lstRolesTodo[ls].IdRol)

        var parametros = {
            "Aplicacion": nombreAplicacionBackbone,
            "ListaIdsRoles": lsRoles,
            "IdUsuario": usuarioDNP,
            "IdObjeto": $sessionStorage.idInstancia,      //'88ea329d-f240-4868-9df7-86c74fb2ecfa',
            "InstanciaId": $sessionStorage.idInstancia,   //'88ea329d-f240-4868-9df7-86c74fb2ecfa',
            "IdFiltro": $sessionStorage.idAccionAnterior
        }

        function init() {            
            vm.model = {
                modulos: {
                    administracion: false,
                    backbone: true
                }
            }
            obtenerSectores();

            if ($sessionStorage.convenioId != undefined)
                editarConvenio();
        }

        function obtenerSectores() {

            datosAdicionDonacionServicio.ObtenerSectores(parametros)
                .then(function (response) {
                    response.data.forEach(item => {
                        vm.listaSectores = response.data;
                    });
                });
        }

        async function cambioSector(entidadIdSeleccionado) {
            var sectorSeleccionado = vm.model.Sector;
            if (entidadIdSeleccionado > 0) {
                sectorSeleccionado = entidadIdSeleccionado;
            }

            var listaEntidades = [];
            return datosAdicionDonacionServicio.ObtenerListaEntidades(parametros, 1)
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    sectorSeleccionado = getIdSector(vm.model.Sector);
                    var arreglolistaEntidades = jQuery.parseJSON(respuesta.data);
                    arreglolistaEntidades = arreglolistaEntidades.filter(item => item.ParentId == sectorSeleccionado)
                    for (var ls = 0; ls < arreglolistaEntidades.length; ls++) {
                        var entidad = {
                            "nombreEntidad": arreglolistaEntidades[ls].Name,
                            "idEntidad": arreglolistaEntidades[ls].Id,
                            "codigo": arreglolistaEntidades[ls].Code
                        }
                        listaEntidades.push(entidad);
                    }
                    vm.listaEntidades = listaEntidades.sort(GetSortOrder("codigo"));

                })
                .catch(error => {
                    console.log(error);
                    toastr.error("Hubo un error al cargar las entidad");
                });
        }

        function guardar() {

            if (!vm.NombreDonante) {
                utilidades.mensajeError("Verifique el nombre del Donante.", false); return false;
            }

            if (!vm.NumeroConvenio) {
                utilidades.mensajeError("Verifique el numero de Convenio.", false); return false;
            }
            
            if (!vm.ValorConvenio) {
                utilidades.mensajeError("Verifique el valor de Convenio.", false); return false;
            }
            else {
                var valConvenio = String(vm.ValorConvenio).split(",");
                valConvenio = valConvenio.toString().replaceAll(".", "");
                if (valConvenio) {
                    if (valConvenio[0].length > 14) {
                        utilidades.mensajeError("Verifique el valor de Convenio.", false); return false;
                    }
                }
            }

            if (!vm.ValorConvenioVigencia) {
                utilidades.mensajeError("Verifique el valor de Convenio por vigencia.", false); return false;
            }
            else {
                var valConvenioVig = String(vm.ValorConvenioVigencia).split(",");
                valConvenioVig = valConvenioVig.toString().replaceAll(".", "");
                if (valConvenioVig) {
                    if (valConvenioVig[0].length > 14) {
                        utilidades.mensajeError("Verifique el valor de Convenio por vigencia.", false); return false;
                    }
                }
            }
            //if (!vm.ValorConvenioVigencia) {
            //    utilidades.mensajeError("Verifique el valor de Convenio por vigencia.", false); return false;
            //}

            if (!vm.Objeto) {
                utilidades.mensajeError("Verifique el valor de Objeto.", false); return false;
            }

            if (!vm.FechaInicial) {
                utilidades.mensajeError("Verifique la Fecha Inicial.", false); return false;
            }

            if (!vm.FechaFinal) {
                utilidades.mensajeError("Verifique la Fecha Final.", false); return false;
            }
            
            var tramiteId = $sessionStorage.tramiteId;
            vm.ValorConvenio = vm.ValorConvenio.replaceAll(".", "");
            vm.ValorConvenio = vm.ValorConvenio.replaceAll(",", ".");
            vm.ValorConvenioVigencia = vm.ValorConvenioVigencia.replaceAll(".", "");
            vm.ValorConvenioVigencia = vm.ValorConvenioVigencia.replaceAll(",", ".");

            var params = {
                Id: 0,
                ConvenioId: 0,
                NombreDonante: vm.NombreDonante,
                EntityId: null,
                objConvenioDto: {
                    Id: 0,
                    TramiteId: tramiteId,
                    NumeroConvenio: vm.NumeroConvenio,
                    ObjetoConvenio: vm.Objeto,
                    ValorConvenio: vm.ValorConvenio,
                    ValorConvenioVigencia: vm.ValorConvenioVigencia,
                    FechaInicial: vm.FechaInicial,
                    FechaFinal: vm.FechaFinal,
                }
            }

            if (vm.ConvenioDonanteId > 0 && vm.ConvenioId > 0) {
                params = {
                    Id: vm.ConvenioId,
                    ConvenioId: vm.ConvenioId,
                    NombreDonante: vm.NombreDonante,
                    EntityId: null,
                    objConvenioDto: {
                        Id: vm.ConvenioDonanteId,
                        TramiteId: tramiteId,
                        NumeroConvenio: vm.NumeroConvenio,
                        ObjetoConvenio: vm.Objeto,
                        ValorConvenio: vm.ValorConvenio,
                        ValorConvenioVigencia: vm.ValorConvenioVigencia,
                        FechaInicial: vm.FechaInicial,
                        FechaFinal: vm.FechaFinal,
                    }
                }
            }

            if (vm.btnGuardarEditar == 'GUARDAR') {
                datosAdicionDonacionServicio.GuardarDatosAdicionDonacion(params, usuarioDNP)
                    .then(function (response) {

                        let exito = response.data;
                        if (exito != undefined) {
                            var respuesta = jQuery.parseJSON(exito);

                            if (respuesta.Exito) {                                
                                utilidades.mensajeSuccess("", false, false, false, "Los datos fueron agregados y guardados con éxito!");
                                $uibModalInstance.close();
                            }
                            else {
                                utilidades.mensajeError("Se presento el siguiente error realizar la operación: " + respuesta.Mensaje, false);
                            }
                        } else
                            utilidades.mensajeError("Se presento un error realizar la operación.", false);
                    })
                    .catch(error => {
                        if (error.status == 400) {
                            utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                            return;
                        }
                        utilidades.mensajeError("Error al realizar la operación");
                    });
            }
            else {
                utilidades.mensajeWarning("Si se realiza esta acción, todos los posibles datos diligenciados en la sección 'Datos de la adición por donación' se perderán. Usted deberá verficar la información incluida previamente en otros espacios sea coherente con la modificación. Esta seguro de continuar?",
                    function funcionContinuar() {

                        return datosAdicionDonacionServicio.GuardarDatosAdicionDonacion(params, usuarioDNP)
                            .then(function (response) {
                                let exito = response.data;
                                if (exito != undefined) {
                                    var respuesta = jQuery.parseJSON(exito);

                                    if (respuesta.Exito) {
                                        vm.cerrar();
                                        utilidades.mensajeSuccess("", false, false, false, "Los datos del convenio se han modificado con éxito!");
                                    }
                                    else {
                                        utilidades.mensajeError("Se presento el siguiente error realizar la operación: " + respuesta.Mensaje, false);
                                    }
                                } else
                                    utilidades.mensajeError("Se presento un error realizar la operación.", false);
                            })
                            .catch(error => {
                                if (error.status == 400) {
                                    utilidades.mensajeError(error.data.Message || "Error al realizar la operación");
                                    vm.limpiarErrores();
                                    return;
                                }
                                utilidades.mensajeError("Error al realizar la operación");
                            }
                                , function funcionCancelar(reason) {
                                    console.log("reason", reason);
                                }
                            );
                    },
                    null,
                    "Aceptar",
                    "Cancelar",
                    "Se actualizaran los datos del convenio."
                );
            }
        }

        //function guardarCapituloModificado() {
        //    var data = {
        //        ProyectoId: $sessionStorage.tramiteId,
        //        Justificacion: "",
        //        SeccionCapituloId: vm.seccionCapitulo.SeccionCapituloId,
        //        InstanciaId: $uibModalInstance.idInstancia,
        //        Modificado: false,
        //    }
        //    datosAdicionDonacionServicio.guardarCambiosFirme(data)
        //        .then(function (response) {
        //            if (response.data.Exito) {
        //                //  vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
        //                //vm.callBack();
        //            }
        //            else {
        //                utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
        //            }
        //        });
        //}

        //function ObtenerSeccionCapitulo() {
        //    const span = document.getElementById('id-capitulo-justificacionjustificacion');
        //    vm.seccionCapitulo = span.textContent;
        //}

        function GetSortOrder(prop) {
            return function (a, b) {
                if (a[prop] > b[prop]) {
                    return 1;
                } else if (a[prop] < b[prop]) {
                    return -1;
                }
                return 0;
            }
        }

        function validateFormat(event) {
            
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            let value = event.target.value.replaceAll(".", "");

            if (value.toString().includes(",")) {
                if (event.keyCode == 44) {
                    event.preventDefault();
                }
                else {
                    let valDecimal = String(value).split(",");

                    if (event.target.selectionStart - 1 >= valDecimal[0].length) {
                        if (valDecimal[1].length >= 2) {
                            event.preventDefault();
                        }
                    }
                    else {
                        if (valDecimal[0].length >= 14) {
                            event.preventDefault();
                        }
                    }
                }
            }
            else {
                if (event.keyCode !== 44) {
                    let tamanioNumber = value.length;
                    if (tamanioNumber >= 14) {
                        event.preventDefault();
                    }
                }
            }
        }

        async function editarConvenio() {
            var convenioId = $sessionStorage.convenioId;
            var listaConvenios = vm.listaDatosAdicion;
            //var sectorIdSeleccionado = 0;
            //var entidadSeleccionado = 0;

            listaConvenios.forEach(function (convenio) {

                if (convenio.ConvenioId == convenioId) {
                    vm.NombreDonante = convenio.NombreDonante;
                    vm.NumeroConvenio = convenio.NumeroConvenio;
                    //vm.ValorConvenio = convenio.ValorConvenio;
                    let valueConvenio = String(convenio.ValorConvenio).split(".");
                    vm.ValorConvenio = valueConvenio[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");
                    if (valueConvenio.length > 1) {
                        vm.ValorConvenio = vm.ValorConvenio + ',' + valueConvenio[1];
                    }
                    let valueConvenioVig = String(convenio.ValorConvenioVigencia).split(".");
                    vm.ValorConvenioVigencia = valueConvenioVig[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");
                    if (valueConvenioVig.length > 1) {
                        vm.ValorConvenioVigencia = vm.ValorConvenioVigencia + ',' + valueConvenioVig[1];
                    }
                    /*vm.ValorConvenioVigencia = convenio.ValorConvenioVigencia;*/
                    vm.Objeto = convenio.ObjetoConvenio;
                    vm.FechaInicial = new Date(convenio.FechaInicial);
                    vm.FechaFinal = new Date(convenio.FechaFinal);
                    vm.ConvenioDonanteId = convenio.ConvenioDonanteId;
                    vm.ConvenioId = convenio.ConvenioId;
                }
            });
            vm.btnGuardarEditar = "ACTUALIZAR";            
        }

        function getIdSector(name) {
            var id = 0;
            vm.listaSectores.forEach(item => {
                if (item.Name == name) {
                    id = item.Id
                }

            });
            return id;
        }

        vm.focus = function (event) {
            event.target.value = event.target.value.replaceAll(".", "");
        }

        vm.blur = function (event) {
            event.target.value = formatoNumero(event);
        }

        function formatoNumero(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            let valConvenioSal;
            let valDecimal;
            let valConvenioEnt = String(event.target.value).split(",");
            if (valConvenioEnt) {

                valConvenioSal = valConvenioEnt[0]
                    .replace(/[^\d,]/g, "")
                    .replace(/^(\d*\,)(.*)\.(.*)$/, '$1$2$3')
                    .replace(/\,(\d{2})\d+/, '.$1')
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ".");

                if (valConvenioEnt.length > 1) {
                    valDecimal = valConvenioEnt[1];

                    valConvenioSal = valConvenioSal + "," + valDecimal;
                }
            }

            return valConvenioSal;
        }

        //function formatearNumero(value) {
        //    var numerotmp = (value == '' || Number.isNaN(value)) ? 0 : value.toString().replaceAll('.', '');
        //    return parseInt(numerotmp).toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ".");
        //}

        //function desFormatearNumero(value) {
        //    return (value == '' || value == 'NaN') ? 0 : Number(value.replace(/[^0-9,-]+/g, ""));
        //}
    }

    angular.module('backbone').controller('modalDatosAdicionDonacionController', modalDatosAdicionDonacionController)        
        .directive('stringToNumber', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    ngModel.$parsers.push(function (value) {

                        return '' + value;
                    });
                    ngModel.$formatters.push(function (value) {
                        return parseFloat(value);
                    });
                }
            };
        })
        ;

})();