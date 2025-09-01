(function () {
    'use strict';

    modalDatosIncorporacionController.$inject = [
        '$sessionStorage', '$uibModalInstance', 'datosIncorporacionServicio', 'utilidades', 'constantesBackbone', '$scope'
    ];

    function modalDatosIncorporacionController(
        $sessionStorage, $uibModalInstance, datosIncorporacionServicio, utilidades, constantesBackbone, $scope
    ) {

        var vm = this;
        vm.init = init;
        vm.cambioSector = cambioSector;
        vm.guardar = guardar;
        vm.cerrar = $uibModalInstance.close;
        vm.seccionCapitulo = null;
        vm.nombreComponente = "proyectodatosincorporacion";
        vm.validateFormat = validateFormat;
        vm.validateFormat2 = validateFormat2;
        vm.convenioId = $sessionStorage.convenioId;
        vm.listaDatosIncorporacion = $sessionStorage.listaDatosIncorporacion
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

            datosIncorporacionServicio.ObtenerSectores(parametros)
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
            return datosIncorporacionServicio.ObtenerListaEntidades(parametros, 1)
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

            if (!vm.model.Sector) {
                utilidades.mensajeError("Verifique el Sector.", false); return false;
            }

            if (!vm.model.idEntidad && !vm.EntityId) {
                utilidades.mensajeError("Verifique la Entidad Aportante.", false); return false;
            }

            if (!vm.NumeroConvenio) {
                utilidades.mensajeError("Verifique el numero de Convenio.", false); return false;
            }

            if (!vm.ValorConvenio) {
                utilidades.mensajeError("Verifique el valor de Convenio.", false); return false;
            }

            if (!vm.ValorConvenioVigencia) {
                utilidades.mensajeError("Verifique el valor de Convenio por vigencia.", false); return false;
            }

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

            var params = {
                Id: 0,
                ConvenioId: 0,
                NombreDonante: vm.model.Sector,        // vm.model.Sector.Name,
                EntityId: vm.model.idEntidad,
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
                    NombreDonante: vm.model.Sector,
                    EntityId: vm.model.idEntidad,
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

            datosIncorporacionServicio.GuardarDatosIncorporacion(params, usuarioDNP)
                .then(function (response) {

                    let exito = response.data;
                    if (exito != undefined) {
                        var respuesta = jQuery.parseJSON(exito);

                        if (respuesta.Exito) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
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

        function guardarCapituloModificado() {
            var data = {
                ProyectoId: $sessionStorage.tramiteId,
                Justificacion: "",
                SeccionCapituloId: vm.seccionCapitulo.SeccionCapituloId,
                InstanciaId: $uibModalInstance.idInstancia,
                Modificado: false,
            }
            datosIncorporacionServicio.guardarCambiosFirme(data)
                .then(function (response) {
                    if (response.data.Exito) {
                        //  vm.guardadoevent({ nombreComponenteHijo: vm.nombreComponente });
                        //vm.callBack();
                    }
                    else {
                        utilidades.mensajeError(response.data.Mensaje + " Capitulo Modificado");
                    }
                });
        }

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-justificacionjustificacion');
            vm.seccionCapitulo = span.textContent;
        }

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

            //let value = parseFloat(event.target.value);
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 11;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 4);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 4) {
                        tamanioPermitido = n[0].length + 4;
                        event.target.value = n[0] + '.' + n[1].slice(0, 4);
                        return;
                    }

                    if ((n[1].length == 3 && n[1] > 999) || (n[1].length > 3 && n[1] > 9999)) {
                        event.preventDefault();
                    }
                }
            } else {

                if (tamanio > 14 && event.keyCode == 51) {
                    event.preventDefault();
                }

                if (tamanio > 12 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio > tamanioPermitido || tamanio > 16) {
                    event.preventDefault();
                }
            }
        }

        function validateFormat2(event) {

            //let value = parseFloat(event.target.value);
            if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 44) {
                event.preventDefault();
            }

            var newValue = event.target.value;
            var spiltArray = String(newValue).split("");
            var tamanioPermitido = 14;
            var tamanio = event.target.value.length;
            var permitido = false;
            permitido = event.target.value.toString().includes(".");
            if (permitido) {
                tamanioPermitido = 16;

                var n = String(newValue).split(".");
                if (n[1]) {
                    var n2 = n[1].slice(0, 2);
                    newValue = [n[0], n2].join(".");
                    if (spiltArray.length === 0) return;
                    if (spiltArray.length === 1 && (spiltArray[0] == '-' || spiltArray[0] === '.')) return;
                    if (spiltArray.length === 2 && newValue === '-.') return;

                    if (n[1].length > 2) {
                        tamanioPermitido = n[0].length;
                        event.target.value = n[0] + '.' + n[1].slice(0, 2);
                        event.preventDefault();
                    }

                    if (n[1].length == 2) {
                        event.preventDefault();
                    }
                }
            } else {

                if (tamanio > 14 && event.keyCode == 51) {
                    event.preventDefault();
                }

                if (tamanio > 14 && event.keyCode != 44) {
                    event.preventDefault();
                }
            }

            if (event.keyCode === 44 && tamanio == 12) {
            }
            else {
                if (tamanio >= tamanioPermitido || tamanio > 16) {
                    event.preventDefault();
                }
            }
        }

        async function editarConvenio() {
            var convenioId = $sessionStorage.convenioId;
            var listaConvenios = vm.listaDatosIncorporacion;
            var sectorIdSeleccionado = 0;
            var entidadSeleccionado = 0;

            listaConvenios.forEach(function (convenio) {

                if (convenio.ConvenioId == convenioId) {

                    vm.model.Sector = convenio.Sector;
                    vm.SectorId = convenio.SectorId;
                    vm.NumeroConvenio = convenio.NumeroConvenio;
                    vm.ValorConvenio = convenio.ValorConvenio;
                    vm.Objeto = convenio.ObjetoConvenio;
                    vm.ValorConvenioVigencia = convenio.ValorConvenioVigencia;
                    vm.FechaInicial = new Date(convenio.FechaInicial);
                    vm.FechaFinal = new Date(convenio.FechaFinal);
                    vm.ConvenioDonanteId = convenio.ConvenioDonanteId;
                    vm.ConvenioId = convenio.ConvenioId;
                    sectorIdSeleccionado = convenio.SectorId;
                    //vm.EntityId = convenio.EntityId;
                    entidadSeleccionado = convenio.EntityId;
                }
            });
            cambioSector(sectorIdSeleccionado);
            vm.EntityId = entidadSeleccionado;
            vm.model.idEntidad = entidadSeleccionado;
            $("#btnguardarDNP").html("ACTUALIZAR");
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

    }

    angular.module('backbone').controller('modalDatosIncorporacionController', modalDatosIncorporacionController)
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
        });

})();
