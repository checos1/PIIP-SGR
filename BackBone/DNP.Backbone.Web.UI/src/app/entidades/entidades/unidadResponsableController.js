(function () {
    'use strict';

    unidadResponsableController.$inject = ['$scope',
        'servicioEntidades',
        'constantesBackbone',
        '$uibModal',
        'FileSaver',
        'utilidades',
        'servicioUsuarios',
        'autorizacionServicios',
        '$localStorage',
        'backboneServicios'
    ];

    function unidadResponsableController($scope, servicioEntidades, constantesBackbone, $uibModal, FileSaver, utilidades, servicioUsuarios, autorizacionServicios, $localStorage, backboneServicios) {
        var vm = this;
        vm.modelo = [];
        vm.modeloSectores = [];
        vm.rowEditando = [];
        vm.rowActivos = [];
        vm.rowEditar = [];
        vm.rowInactivos = [];
        vm.flujoSinDefinitiva = [];
        vm.flujoConDefinitiva = [];        
        vm.listSector = [];
        vm.modeloFlujoViabilidad = [];
        vm.tipoFlujoSeleccionado = [];
        vm.configuracionSectorEntidades = [];
        vm.matrizEntidadConfiguracion = [];
        vm.buscarIdentificacion = [];
        vm.buscarNombre = [];
        vm.sinResultadosUsuario = [];
        vm.resultadosUsuario = [];
        vm.resultadosUsuarioContador = [];
        vm.resultadosUsuariosBusqueda = [];
        vm.editarDetalleUsuarioRow = [];
        vm.nombreUsuarioEditar = [];
        vm.tipoUsuarioEditar = [];
        vm.identificacionUsuarioEditar = [];
        vm.usuarioDNPEditar = [];
        vm.editarConfiguracionUsuarioRow = [];
        vm.editarConfiguracionUsuario = [];
        vm.estadoParaEditarUsuario = [];
        vm.rowEditarUsuario = [];
        vm.idUsuarioSeleccionado = [];
        vm.unidadesResponsablesModal = [];
        
        vm.rolGuidVR = constantesBackbone.idRControlFormulacion;
        vm.rolGuidVD = constantesBackbone.idRControlPosteriorViabilidad;
        vm.rolGuidAP = constantesBackbone.idRFormulador;
        vm.rolGuidASP = constantesBackbone.idRolPresupuesto;
        vm.rolGuidFE = constantesBackbone.idRFirmaConceptoViabilidad;
        
        vm.ultimoIdEntidad = '';
        vm.nombreTabUtilizado = '';
        vm.abrirMensajeInformacionRegionalizacion = abrirMensajeInformacionRegionalizacion;
        vm.abrirMensajeConfiguracionFlujo = abrirMensajeConfiguracionFlujo;
        
        vm.enabledSector = function (sectorId, entidadId, tipo, cancelar = 0) {
            // Verificación de requisitos
            if (tipo == 'VR') {
                var estadoVR = document.getElementById("valueVR-" + entidadId + "-" + sectorId).value;
                if (estadoVR == '0') {
                    document.querySelectorAll('div[id=SVR-' + sectorId + '-' + entidadId +']').forEach(element => {
                        element.removeAttribute("hidden")
                    });
                    document.getElementById("valueVR-" + entidadId + "-" + sectorId).value = '1';
                }
                else {
                    document.querySelectorAll('div[id=SVR-' + sectorId + '-' + entidadId + ']').forEach(element => {
                        element.setAttribute("hidden", true);
                    });
                    document.getElementById("valueVR-" + entidadId + "-" + sectorId).value = '0';
                }
            }
            // Viabilidad definitiva
            else if (tipo == 'VD') {
                var estadoVD = document.getElementById("valueVD-" + entidadId + "-" + sectorId).value;
                if (estadoVD == '0') {
                    document.querySelectorAll('div[id=SVD-' + sectorId + '-' + entidadId + ']').forEach(element => {
                        element.removeAttribute("hidden")
                    });
                    document.getElementById("valueVD-" + entidadId + "-" + sectorId).value = '1';
                }
                else {
                    document.querySelectorAll('div[id=SVD-' + sectorId + '-' + entidadId + ']').forEach(element => {
                        element.setAttribute("hidden", true);
                    });
                    document.getElementById("valueVD-" + entidadId + "-" + sectorId).value = '0';
                }
            }
            //Ajustes proyecto
            else if (tipo == 'AP') {
                var estadoAP = document.getElementById("valueVAP-" + entidadId + "-" + sectorId).value;
                if (estadoAP == '0') {
                    document.querySelectorAll('div[id=SAP-' + sectorId + '-' + entidadId + ']').forEach(element => {
                        element.removeAttribute("hidden")
                    });
                    document.getElementById("valueVAP-" + entidadId + "-" + sectorId).value = '1';
                }
                else {
                    document.querySelectorAll('div[id=SAP-' + sectorId + '-' + entidadId + ']').forEach(element => {
                        element.setAttribute("hidden", true);
                    });
                    document.getElementById("valueVAP-" + entidadId + "-" + sectorId).value = '0';
                }
            }
            // Asociar proyectos
            else if (tipo == 'ASP') {
                var estadoASP = document.getElementById("valueVASP-" + entidadId + "-" + sectorId).value;
                if (estadoASP == '0') {
                    document.querySelectorAll('div[id=SASP-' + sectorId + '-' + entidadId + ']').forEach(element => {
                        element.removeAttribute("hidden")
                    });
                    document.getElementById("valueVASP-" + entidadId + "-" + sectorId).value = '1';
                }
                else {
                    document.querySelectorAll('div[id=SASP-' + sectorId + '-' + entidadId + ']').forEach(element => {
                        element.setAttribute("hidden", true);
                    });
                    document.getElementById("valueVASP-" + entidadId + "-" + sectorId).value = '0';
                }
             }

            if (cancelar == 1) {
                var entidad = vm.modelo.find(x => x.IdEntidad == entidadId);
                if (entidad == null || entidad == '' || entidad == undefined)
                    return false;
                var entidadSector = entidad.ConfiguracionMatriz.find(y => y.SectorId == sectorId);
                if (tipo == 'VR') {
                    if (entidadSector.DataVR.length > 0) {
                        for (var z = 0; z < entidadSector.DataVR.length; z++) {
                            if (entidadSector.DataVR[z].Id == 0) {
                                entidadSector.DataVR[z].Estado = 2;
                                document.getElementById("chkR-" + sectorId + "-" + entidadId + "-" + entidadSector.DataVR[z].EntidadDestinoId).checked = false;
                            }
                            else {
                                entidadSector.DataVR[z].Estado = 0;
                                document.getElementById("chkR-" + sectorId + "-" + entidadId + "-" + entidadSector.DataVR[z].EntidadDestinoId).checked = true;
                            }
                        }
                    }
                    else {
                        for (var i = 0; i < entidadSector.ListaModalVR.length; i++) {
                            document.getElementById("chkR-" + sectorId + "-" + entidadId + "-" + entidadSector.ListaModalVR[i].EntityTypeCatalogOptionId).checked = false;
                        }
                    }                    
                }
                else if (tipo == 'VD') {
                    if (entidadSector.DataVD.length > 0) {
                        for (var z = 0; z < entidadSector.DataVD.length; z++) {
                            if (entidadSector.DataVD[z].Id == 0) {
                                entidadSector.DataVD[z].Estado = 2;
                                document.getElementById("chkD-" + sectorId + "-" + entidadId + "-" + entidadSector.DataVD[z].EntidadDestinoId).checked = false;
                            }
                            else {
                                entidadSector.DataVD[z].Estado = 0;
                                document.getElementById("chkD-" + sectorId + "-" + entidadId + "-" + entidadSector.DataVD[z].EntidadDestinoId).checked = true;
                            }
                        }

                        for (var z = 0; z < entidadSector.DataFE.length; z++) {
                            if (entidadSector.DataFE[z].Id == 0) {
                                entidadSector.DataFE[z].Estado = 2;
                            }
                            else {
                                entidadSector.DataFE[z].Estado = 0;
                            }
                        }
                    }
                    else {
                        for (var i = 0; i < entidadSector.ListaModalVD.length; i++) {
                            document.getElementById("chkD-" + sectorId + "-" + entidadId + "-" + entidadSector.ListaModalVD[i].EntityTypeCatalogOptionId).checked = false;
                        }
                    }                    
                }
                else if (tipo == 'AP') {
                    if (entidadSector.DataAP.length > 0) {
                        for (var z = 0; z < entidadSector.DataAP.length; z++) {
                            if (entidadSector.DataAP[z].Id == 0) {
                                entidadSector.DataAP[z].Estado = 2;
                                document.getElementById("chkAP-" + sectorId + "-" + entidadId + "-" + entidadSector.DataAP[z].EntidadDestinoId).checked = false;
                            }
                            else {
                                entidadSector.DataAP[z].Estado = 0;
                                document.getElementById("chkAP-" + sectorId + "-" + entidadId + "-" + entidadSector.DataAP[z].EntidadDestinoId).checked = true;
                            }
                        }
                    }
                    else {
                        for (var i = 0; i < entidadSector.ListaModalAP.length; i++) {
                            document.getElementById("chkAP-" + sectorId + "-" + entidadId + "-" + entidadSector.ListaModalAP[i].EntityTypeCatalogOptionId).checked = false;
                        }
                    }
                }
                else if (tipo == 'ASP') {
                    if (entidadSector.DataASP.length > 0) {
                        for (var z = 0; z < entidadSector.DataASP.length; z++) {
                            if (entidadSector.DataASP[z].Id == 0) {
                                entidadSector.DataASP[z].Estado = 2;
                                document.getElementById("chkASP-" + sectorId + "-" + entidadId + "-" + entidadSector.DataASP[z].EntidadDestinoId).checked = false;
                            }
                            else {
                                entidadSector.DataVR[z].Estado = 0;
                                document.getElementById("chkASP-" + sectorId + "-" + entidadId + "-" + entidadSector.DataASP[z].EntidadDestinoId).checked = true;
                            }
                        }
                    }
                    else {
                        for (var i = 0; i < entidadSector.ListaModalASP.length; i++) {
                            document.getElementById("chkASP-" + sectorId + "-" + entidadId + "-" + entidadSector.ListaModalASP[i].EntityTypeCatalogOptionId).checked = false;
                        }
                    }
                }
            }            
        }

        vm.ActivarEditar = function (entidadId) {
            var entidad = vm.modelo.find(x => x.IdEntidad == entidadId);
            if (entidad == null || entidad == '' || entidad == undefined) {
                utilidades.mensajeError("Al menos una unidad responsable debe estar marcada como 'Asume rol de presupuesto'.", false);
                return false;
            }
            var exiteListaEntidad = (entidad.ConfiguracionMatriz == undefined || entidad.ConfiguracionMatriz == '' || entidad.ConfiguracionMatriz == null) ? []
                : entidad.ConfiguracionMatriz.find(x => x.EntidadResponsable == entidad.EntityTypeCatalogOptionId);
            if (exiteListaEntidad.length == 0)
                return false;
            if (vm.rowEditar[entidadId] == false) {
                vm.rowEditar[entidadId] = true;
                $("#GuardarFlujo-" + entidadId).attr('disabled', false);

                document.querySelectorAll('button[id=btnAddVR-' + entidadId + ']').forEach(element => {
                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnAddVRD-' + entidadId + ']').forEach(element => {
                    element.setAttribute("hidden", true);
                });

                document.querySelectorAll('button[id=btnAddVD-' + entidadId + ']').forEach(element => {
                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnAddVDD-' + entidadId + ']').forEach(element => {
                    element.setAttribute("hidden", true);
                });

                document.querySelectorAll('button[id=btnAddAP-' + entidadId + ']').forEach(element => {
                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnAddAPD-' + entidadId + ']').forEach(element => {
                    element.setAttribute("hidden", true);
                });

                document.querySelectorAll('button[id=btnAddASP-' + entidadId + ']').forEach(element => {
                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnAddASPD-' + entidadId + ']').forEach(element => {
                    element.setAttribute("hidden", true);
                });
            } else {

                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    vm.rowEditar[entidadId] = false;
                    $("#GuardarFlujo-" + entidadId).attr('disabled', true);

                      document.querySelectorAll('button[id=btnAddVRD-' + entidadId + ']').forEach(element => {
                        element.removeAttribute("hidden")
                    });
                    document.querySelectorAll('button[id=btnAddVR-' + entidadId + ']').forEach(element => {
                        element.setAttribute("hidden", true);
                    });

                    document.querySelectorAll('button[id=btnAddVDD-' + entidadId + ']').forEach(element => {
                        element.removeAttribute("hidden")
                    });
                    document.querySelectorAll('button[id=btnAddVD-' + entidadId + ']').forEach(element => {
                        element.setAttribute("hidden", true);
                    });

                    document.querySelectorAll('button[id=btnAddAPD-' + entidadId + ']').forEach(element => {
                        element.removeAttribute("hidden")
                    });
                    document.querySelectorAll('button[id=btnAddAP-' + entidadId + ']').forEach(element => {
                        element.setAttribute("hidden", true);
                    });

                    document.querySelectorAll('button[id=btnAddASPD-' + entidadId + ']').forEach(element => {
                        element.removeAttribute("hidden")
                    });
                    document.querySelectorAll('button[id=btnAddASP-' + entidadId + ']').forEach(element => {
                        element.setAttribute("hidden", true);
                    });

                    var entidad = vm.modelo.find(x => x.IdEntidad == entidadId);

                    for (var x = 0; x < vm.modeloSectores.length; x++) {

                        document.querySelectorAll('div[id=SVR-' + vm.modeloSectores[x].SectorNegocioId + '-' + entidadId + ']').forEach(element => {
                            element.setAttribute("hidden", true);
                        });

                        document.querySelectorAll('div[id=SVD-' + vm.modeloSectores[x].SectorNegocioId + '-' + entidadId + ']').forEach(element => {
                            element.setAttribute("hidden", true);
                        });

                        document.querySelectorAll('div[id=SAP-' + vm.modeloSectores[x].SectorNegocioId + '-' + entidadId + ']').forEach(element => {
                            element.setAttribute("hidden", true);
                        });

                        document.querySelectorAll('div[id=SASP-' + vm.modeloSectores[x].SectorNegocioId + '-' + entidadId + ']').forEach(element => {
                            element.setAttribute("hidden", true);
                        });                        

                        document.getElementById("valueVR-" + entidadId + "-" + vm.modeloSectores[x].SectorNegocioId).value = '0';
                        document.getElementById("valueVD-" + entidadId + "-" + vm.modeloSectores[x].SectorNegocioId).value = '0';
                        document.getElementById("valueVAP-" + entidadId + "-" + vm.modeloSectores[x].SectorNegocioId).value = '0';
                        document.getElementById("valueVASP-" + entidadId + "-" + vm.modeloSectores[x].SectorNegocioId).value = '0';

                        var entidadSector = entidad.ConfiguracionMatriz.find(y => y.SectorId == vm.modeloSectores[x].SectorNegocioId);

                        for (var z = 0; z < entidadSector.DataVR.length; z++) {
                            if (entidadSector.DataVR[z].Id == 0) {
                                entidadSector.DataVR[z].Estado = 2;
                                document.getElementById("chkR-" + vm.modeloSectores[x].SectorNegocioId + "-" + entidadId + "-" + entidadSector.DataVR[z].EntidadDestinoId).checked = false;
                            }
                            else {
                                entidadSector.DataVR[z].Estado = 0;
                                document.getElementById("chkR-" + vm.modeloSectores[x].SectorNegocioId + "-" + entidadId + "-" + entidadSector.DataVR[z].EntidadDestinoId).checked = true;
                            }
                        }

                        for (var z = 0; z < entidadSector.DataVD.length; z++) {
                            if (entidadSector.DataVD[z].Id == 0) {
                                entidadSector.DataVD[z].Estado = 2;
                                document.getElementById("chkD-" + vm.modeloSectores[x].SectorNegocioId + "-" + entidadId + "-" + entidadSector.DataVD[z].EntidadDestinoId).checked = false;

                                entidadSector.DataFE[z].Estado = 2;
                            }
                            else {
                                entidadSector.DataVD[z].Estado = 0;
                                document.getElementById("chkD-" + vm.modeloSectores[x].SectorNegocioId + "-" + entidadId + "-" + entidadSector.DataVD[z].EntidadDestinoId).checked = true;

                                entidadSector.DataFE[z].Estado = 0;
                            }
                        }

                        for (var z = 0; z < entidadSector.DataAP.length; z++) {
                            if (entidadSector.DataAP[z].Id == 0) {
                                entidadSector.DataAP[z].Estado = 2;
                                document.getElementById("chkAP-" + vm.modeloSectores[x].SectorNegocioId + "-" + entidadId + "-" + entidadSector.DataAP[z].EntidadDestinoId).checked = false;
                            }
                            else {
                                entidadSector.DataAP[z].Estado = 0;
                                document.getElementById("chkAP-" + vm.modeloSectores[x].SectorNegocioId + "-" + entidadId + "-" + entidadSector.DataAP[z].EntidadDestinoId).checked = true;
                            }
                        }

                        for (var z = 0; z < entidadSector.DataASP.length; z++) {
                            if (entidadSector.DataASP[z].Id == 0) {
                                entidadSector.DataASP[z].Estado = 2;
                                document.getElementById("chkASP-" + vm.modeloSectores[x].SectorNegocioId + "-" + entidadId + "-" + entidadSector.DataASP[z].EntidadDestinoId).checked = false;
                            }
                            else {
                                entidadSector.DataASP[z].Estado = 0;
                                document.getElementById("chkASP-" + vm.modeloSectores[x].SectorNegocioId + "-" + entidadId + "-" + entidadSector.DataASP[z].EntidadDestinoId).checked = true;
                            }
                        }
                    }

                    vm.ExitoCancelar();

                }, function funcionCancelar(reason) {
                    console.log("reason", reason);
                }, null, null, "Los datos que posiblemente haya diligenciado en la tabla se perderán.");
            }           
        }

        vm.ExitoCancelar = function () {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito");
            }, 500);
        }

        vm.AbrilNivel1 = function (entidadId) {
            var variable = $("#ico" + entidadId)[0].innerText;
            variable = variable.replace(/ /g, "");
            var imgmas = document.getElementById("imgmas" + entidadId);
            var imgmenos = document.getElementById("imgmenos" + entidadId);
            if (variable === "+") {
                $("#ico" + entidadId).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';

            } else {
                $("#ico" + entidadId).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        /// Comienzo
        vm.init = function () {
            vm.listEntidadDestino = [];
            vm.modeloFlujoViabilidad = [];
            vm.modeloSectores = [];
            vm.modelo = [];
            vm.listSector = [];
            vm.configuracionSectorEntidades = [];

            //servicioEntidades.obtenerFlowCatalogSgp().then(
            //    function (response) {
            //        var arreglolistas = jQuery.parseJSON(response.data);
            //        var arreglolistas2 = jQuery.parseJSON(arreglolistas);
            //    });

            servicioEntidades.obtenerSectoresSgp().then(
                function (response) {
                    var arreglolistas = jQuery.parseJSON(response.data);
                    var arreglolistas2 = jQuery.parseJSON(arreglolistas);
                    vm.modeloSectores = arreglolistas2.Resultados;
                    for (var i = 0; i < vm.modeloSectores.length; i++) {
                        vm.listSector.push({ SectorId: vm.modeloSectores[i].SectorNegocioId });
                    }
                    servicioEntidades.obtenerEntidadesPorUnidadesResponsables()
                        .then(function (response) {
                            vm.modelo = response.data.Resultados;
                            for (var i = 0; i < vm.modelo.length; i++) {
                                vm.rowActivos[vm.modelo[i].IdEntidad] = 0;
                                vm.rowInactivos[vm.modelo[i].IdEntidad] = 0;
                                vm.rowEditar[vm.modelo[i].IdEntidad] = false;
                                vm.estadoParaEditarUsuario[vm.modelo[i].IdEntidad] = 0;
                                vm.flujoSinDefinitiva[vm.modelo[i].IdEntidad] = false;
                                vm.flujoConDefinitiva[vm.modelo[i].IdEntidad] = false;
                                if (vm.modelo[i].UnidadesResponsables != null && vm.modelo[i].UnidadesResponsables != undefined) {
                                    
                                    for (var j = 0; j < vm.modelo[i].UnidadesResponsables.length; j++) {
                                        if (vm.modelo[i].UnidadesResponsables[j].EntityTypeCatalogOptionId != undefined && vm.modelo[i].UnidadesResponsables[j].EntityTypeCatalogOptionId != null) {
                                            vm.listEntidadDestino.push({ EntidadDestinoId: vm.modelo[i].UnidadesResponsables[j].EntityTypeCatalogOptionId });
                                        }
                                        if (vm.modelo[i].UnidadesResponsables[j].IsActivo) {
                                            vm.rowActivos[vm.modelo[i].IdEntidad] = vm.rowActivos[vm.modelo[i].IdEntidad] + 1;
                                        }
                                        else {
                                            vm.rowInactivos[vm.modelo[i].IdEntidad] = vm.rowInactivos[vm.modelo[i].IdEntidad] + 1;
                                        }
                                        
                                    }
                                    vm.modeloFlujoViabilidad.push({
                                        ListEntidadDestinoId: vm.listEntidadDestino,
                                        EntidadResponsableId: vm.modelo[i].EntityTypeCatalogOptionId,
                                        ResourceGroupId: 5,
                                        ListSectorId: vm.listSector
                                    });
                                }                              
                            }
                            servicioEntidades.obtenerMatrizEntidadDestino(vm.modeloFlujoViabilidad)
                                .then(function (response) {
                                    for (var i = 0; i < vm.modelo.length; i++) {
                                        var unidadesResponsablesList = (vm.modelo[i].UnidadesResponsables == undefined || vm.modelo[i].UnidadesResponsables == null || vm.modelo[i].UnidadesResponsables == '')
                                            ? [] : vm.modelo[i].UnidadesResponsables.filter(elem => elem.IsActivo == true);

                                        for (var k = 0; k < vm.modeloSectores.length; k++) {
                                            var encontro = false;
                                            if (response.data != null) {
                                                for (var j = 0; j < response.data.length; j++) {
                                                    if (vm.modelo[i].EntityTypeCatalogOptionId == response.data[j].EntidadResponsableId) {
                                                        encontro = true;

                                                        //Verificación de requisitos
                                                        var dataR = response.data[j].Respuesta.filter(elem => elem.SectorId == vm.modeloSectores[k].SectorNegocioId && elem.RolId.toUpperCase() == vm.rolGuidVR && elem.EntidadResponsableId == response.data[j].EntidadResponsableId);

                                                        vm.unidadesResponsablesModalVR = [];
                                                        for (var p = 0; p < unidadesResponsablesList.length; p++) {
                                                            var check = false;
                                                            vm.modelo = vm.modelo.filter(x => unidadesResponsablesList[p].EntityTypeCatalogOptionId != x.EntityTypeCatalogOptionId);
                                                            for (var q = 0; q < dataR.length; q++) {
                                                                if (dataR[q].EntidadDestinoId == unidadesResponsablesList[p].EntityTypeCatalogOptionId) {
                                                                    check = true;
                                                                    q = dataR.length
                                                                }

                                                            }
                                                            vm.unidadesResponsablesModalVR.push({
                                                                EntityTypeCatalogOptionId: unidadesResponsablesList[p].EntityTypeCatalogOptionId,
                                                                Nombre: unidadesResponsablesList[p].Nombre,
                                                                Seleccionado: check
                                                            })
                                                        }

                                                        //Viabilidad definitiva
                                                        var dataD = response.data[j].Respuesta.filter(elem => elem.SectorId == vm.modeloSectores[k].SectorNegocioId && elem.RolId.toUpperCase() == vm.rolGuidVD && elem.EntidadResponsableId == response.data[j].EntidadResponsableId);

                                                        vm.unidadesResponsablesModalVD = [];
                                                        for (var r = 0; r < unidadesResponsablesList.length; r++) {
                                                            var check = false;
                                                            for (var s = 0; s < dataD.length; s++) {
                                                                if (dataD[s].EntidadDestinoId == unidadesResponsablesList[r].EntityTypeCatalogOptionId) {
                                                                    check = true;
                                                                    s = dataD.length
                                                                }

                                                            }
                                                            vm.unidadesResponsablesModalVD.push({
                                                                EntityTypeCatalogOptionId: unidadesResponsablesList[r].EntityTypeCatalogOptionId,
                                                                Nombre: unidadesResponsablesList[r].Nombre,
                                                                Seleccionado: check
                                                            })
                                                        }

                                                        //Ajustes proyecto
                                                        var dataAP = response.data[j].Respuesta.filter(elem => elem.SectorId == vm.modeloSectores[k].SectorNegocioId && elem.RolId.toUpperCase() == vm.rolGuidAP && elem.EntidadResponsableId == response.data[j].EntidadResponsableId);

                                                        vm.unidadesResponsablesModalAP = [];
                                                        for (var m = 0; m < unidadesResponsablesList.length; m++) {
                                                            var check = false;
                                                            for (var n = 0; n < dataAP.length; n++) {
                                                                if (dataAP[n].EntidadDestinoId == unidadesResponsablesList[m].EntityTypeCatalogOptionId) {
                                                                    check = true;
                                                                    n = dataAP.length
                                                                }

                                                            }
                                                            vm.unidadesResponsablesModalAP.push({
                                                                EntityTypeCatalogOptionId: unidadesResponsablesList[m].EntityTypeCatalogOptionId,
                                                                Nombre: unidadesResponsablesList[m].Nombre,
                                                                Seleccionado: check
                                                            })
                                                        }

                                                        //Asociar proyectos
                                                        var dataS = response.data[j].Respuesta.filter(elem => elem.SectorId == vm.modeloSectores[k].SectorNegocioId && elem.RolId.toUpperCase() == vm.rolGuidASP && elem.EntidadResponsableId == response.data[j].EntidadResponsableId);

                                                        vm.unidadesResponsablesModalASP = [];
                                                        for (var m = 0; m < unidadesResponsablesList.length; m++) {
                                                            var check = false;
                                                            for (var n = 0; n < dataS.length; n++) {
                                                                if (dataS[n].EntidadDestinoId == unidadesResponsablesList[m].EntityTypeCatalogOptionId) {
                                                                    check = true;
                                                                    n = dataS.length
                                                                }
                                                            }
                                                            vm.unidadesResponsablesModalASP.push({
                                                                EntityTypeCatalogOptionId: unidadesResponsablesList[m].EntityTypeCatalogOptionId,
                                                                Nombre: unidadesResponsablesList[m].Nombre,
                                                                Seleccionado: check
                                                            })
                                                        }

                                                        //Firma y Emisión
                                                        var dataFE = response.data[j].Respuesta.filter(elem => elem.SectorId == vm.modeloSectores[k].SectorNegocioId && elem.RolId.toUpperCase() == vm.rolGuidFE && elem.EntidadResponsableId == response.data[j].EntidadResponsableId);

                                                        vm.unidadesResponsablesModalFE = [];
                                                        for (var m = 0; m < unidadesResponsablesList.length; m++) {
                                                            var check = false;
                                                            for (var n = 0; n < dataFE.length; n++) {
                                                                if (dataFE[n].EntidadDestinoId == unidadesResponsablesList[m].EntityTypeCatalogOptionId) {
                                                                    check = true;
                                                                    n = dataFE.length
                                                                }

                                                            }
                                                            vm.unidadesResponsablesModalFE.push({
                                                                EntityTypeCatalogOptionId: unidadesResponsablesList[m].EntityTypeCatalogOptionId,
                                                                Nombre: unidadesResponsablesList[m].Nombre,
                                                                Seleccionado: check
                                                            })
                                                        }

                                                        vm.configuracionSectorEntidades.push({
                                                            EntidadResponsable: vm.modelo[i].EntityTypeCatalogOptionId,
                                                            ListaEntidad: response.data[j].Respuesta,
                                                            TipoFlujo: response.data[j].TipoFlujo,
                                                            SectorId: vm.modeloSectores[k].SectorNegocioId,
                                                            SectorName: vm.modeloSectores[k].Nombre,                                                            
                                                            DataVR: dataR,
                                                            DataVD: dataD,
                                                            DataAP: dataAP,
                                                            DataASP: dataS,
                                                            DataFE: dataFE,                                                            
                                                            ListaModalVR: vm.unidadesResponsablesModalVR, 
                                                            ListaModalVD: vm.unidadesResponsablesModalVD,
                                                            ListaModalAP: vm.unidadesResponsablesModalAP,
                                                            ListaModalASP: vm.unidadesResponsablesModalASP,
                                                            ListaModalFE: vm.unidadesResponsablesModalFE
                                                        });

                                                        if (response.data[j].FlowId.toUpperCase() == 'A2B51530-559C-47C4-97D4-E433619268AA') {
                                                            vm.tipoFlujoSeleccionado[vm.modelo[i].IdEntidad] = 'A2B51530-559C-47C4-97D4-E433619268AA';
                                                            vm.flujoSinDefinitiva[vm.modelo[i].IdEntidad] = true;
                                                        }
                                                        else if (response.data[j].FlowId.toUpperCase() == '26953581-213F-952E-BB04-41399C4BD1FA') {
                                                            vm.tipoFlujoSeleccionado[vm.modelo[i].IdEntidad] = '26953581-213F-952E-BB04-41399C4BD1FA';
                                                            vm.flujoConDefinitiva[vm.modelo[i].IdEntidad] = true;
                                                        }
                                                        j = response.data.length;
                                                    }
                                                }
                                            }
                                            
                                            if (!encontro) {

                                                vm.unidadesResponsablesModalVR = [];
                                                for (var p = 0; p < unidadesResponsablesList.length; p++) {
                                                    var check = false;
                                                    
                                                    vm.unidadesResponsablesModalVR.push({
                                                        EntityTypeCatalogOptionId: unidadesResponsablesList[p].EntityTypeCatalogOptionId,
                                                        Nombre: unidadesResponsablesList[p].Nombre,
                                                        Seleccionado: check
                                                    })
                                                }
                                                
                                                vm.unidadesResponsablesModalVD = [];
                                                for (var r = 0; r < unidadesResponsablesList.length; r++) {
                                                    var check = false;
                                                    
                                                    vm.unidadesResponsablesModalVD.push({
                                                        EntityTypeCatalogOptionId: unidadesResponsablesList[r].EntityTypeCatalogOptionId,
                                                        Nombre: unidadesResponsablesList[r].Nombre,
                                                        Seleccionado: check
                                                    })
                                                }

                                                vm.unidadesResponsablesModalAP = [];
                                                for (var m = 0; m < unidadesResponsablesList.length; m++) {
                                                    var check = false;
                                                    vm.unidadesResponsablesModalAP.push({
                                                        EntityTypeCatalogOptionId: unidadesResponsablesList[m].EntityTypeCatalogOptionId,
                                                        Nombre: unidadesResponsablesList[m].Nombre,
                                                        Seleccionado: check
                                                    })
                                                }

                                                vm.unidadesResponsablesModalASP = [];
                                                for (var m = 0; m < unidadesResponsablesList.length; m++) {
                                                    var check = false;
                                                    vm.unidadesResponsablesModalASP.push({
                                                        EntityTypeCatalogOptionId: unidadesResponsablesList[m].EntityTypeCatalogOptionId,
                                                        Nombre: unidadesResponsablesList[m].Nombre,
                                                        Seleccionado: check
                                                    })
                                                }

                                                vm.unidadesResponsablesModalFE = [];
                                                for (var m = 0; m < unidadesResponsablesList.length; m++) {
                                                    var check = false;
                                                    vm.unidadesResponsablesModalFE.push({
                                                        EntityTypeCatalogOptionId: unidadesResponsablesList[m].EntityTypeCatalogOptionId,
                                                        Nombre: unidadesResponsablesList[m].Nombre,
                                                        Seleccionado: check
                                                    })
                                                }

                                                vm.configuracionSectorEntidades.push({
                                                    EntidadResponsable: vm.modelo[i].EntityTypeCatalogOptionId,
                                                    ListaEntidad: [],
                                                    TipoFlujo: '',
                                                    SectorId: vm.modeloSectores[k].SectorNegocioId,
                                                    SectorName: vm.modeloSectores[k].Nombre,                                                    
                                                    DataVR : [],
                                                    DataVD: [],
                                                    DataAP: [],
                                                    DataASP : [],
                                                    DataFE: [],                                                    
                                                    ListaModalVR: vm.unidadesResponsablesModalVR,                                                    
                                                    ListaModalVD: vm.unidadesResponsablesModalVD,
                                                    ListaModalAP: vm.unidadesResponsablesModalAP,
                                                    ListaModalASP: vm.unidadesResponsablesModalASP,
                                                    ListaModalFE: vm.unidadesResponsablesModalFE
                                                });
                                            }
                                        }

                                        vm.modelo[i].ConfiguracionMatriz = vm.configuracionSectorEntidades;
                                        vm.configuracionSectorEntidades = [];
                                        vm.modelo = vm.modelo.filter(x => x.UnidadesResponsables != null);
                                        servicioEntidades.notificarCambio({ modelo: vm.modelo });
                                    }
                                    if (vm.ultimoIdEntidad != '') {
                                        vm.AbrilNivel1(vm.ultimoIdEntidad);
                                        $('#obj' + vm.ultimoIdEntidad).collapse("toggle");
                                        if (vm.nombreTabUtilizado!= '')
                                            $("#" + vm.nombreTabUtilizado + vm.ultimoIdEntidad).tab('show');
                                        vm.nombreTabUtilizado = '';
                                    }
                                    
                                });
                        });
                });           
            
        }

        vm.procesarUnidadesSeleccionadasFlujo = function (sector, tipo, idEntidad) {
            if (tipo == 'VR') {
                for (var i = 0; i < sector.ListaModalVR.length; i++) {
                    var estado = $("#chkR-" + sector.SectorId + "-" + idEntidad + "-" + sector.ListaModalVR[i].EntityTypeCatalogOptionId).is(":checked");
                    var encontro = false;
                    for (var j = 0; j < sector.DataVR.length; j++) {
                        if (sector.ListaModalVR[i].EntityTypeCatalogOptionId == sector.DataVR[j].EntidadDestinoId) {
                            encontro = true;
                            if (!estado) {
                                sector.DataVR[j].Estado = 2;
                            }
                            else {
                                sector.DataVR[j].Estado = 0;
                            }
                        }
                    }
                    if (!encontro && estado) {
                        sector.DataVR.push({
                            Id: 0,
                            EntidadDestinoId: sector.ListaModalVR[i].EntityTypeCatalogOptionId,
                            EntidadDestinoAccion: sector.ListaModalVR[i].Nombre,
                            Estado: 0
                        });
                    }
                }
                vm.enabledSector(sector.SectorId, idEntidad, 'VR');
            }            
            else if (tipo == 'VD') {
                for (var i = 0; i < sector.ListaModalVD.length; i++) {
                    var estado = $("#chkD-" + sector.SectorId + "-" + idEntidad + "-" + sector.ListaModalVD[i].EntityTypeCatalogOptionId).is(":checked");
                    var encontro = false;
                    for (var j = 0; j < sector.DataVD.length; j++) {
                        if (sector.ListaModalVD[i].EntityTypeCatalogOptionId == sector.DataVD[j].EntidadDestinoId) {
                            encontro = true;
                            if (!estado) {
                                sector.DataVD[j].Estado = 2;
                            }
                            else {
                                sector.DataVD[j].Estado = 0;
                            }
                        }                        
                    }
                    if (!encontro && estado) {
                        sector.DataVD.push({
                            Id: 0,
                            EntidadDestinoId: sector.ListaModalVD[i].EntityTypeCatalogOptionId,
                            EntidadDestinoAccion: sector.ListaModalVD[i].Nombre,
                            Estado: 0
                        });
                    }
                }
                sector.DataFE = sector.DataVD;
                vm.enabledSector(sector.SectorId, idEntidad, 'VD');
            }
            else if (tipo == 'AP') {
                for (var i = 0; i < sector.ListaModalAP.length; i++) {
                    var estado = $("#chkAP-" + sector.SectorId + "-" + idEntidad + "-" + sector.ListaModalAP[i].EntityTypeCatalogOptionId).is(":checked");
                    var encontro = false;
                    for (var j = 0; j < sector.DataAP.length; j++) {
                        if (sector.ListaModalAP[i].EntityTypeCatalogOptionId == sector.DataAP[j].EntidadDestinoId) {
                            encontro = true;
                            if (!estado) {
                                sector.DataAP[j].Estado = 2;
                            }
                            else {
                                sector.DataAP[j].Estado = 0;
                            }
                        }
                    }
                    if (!encontro && estado) {
                        sector.DataAP.push({
                            Id: 0,
                            EntidadDestinoId: sector.ListaModalAP[i].EntityTypeCatalogOptionId,
                            EntidadDestinoAccion: sector.ListaModalAP[i].Nombre,
                            Estado: 0
                        });
                    }
                }
                vm.enabledSector(sector.SectorId, idEntidad, 'AP');
            }
            else if (tipo == 'ASP') {
                for (var i = 0; i < sector.ListaModalASP.length; i++) {
                    var estado = $("#chkASP-" + sector.SectorId + "-" + idEntidad + "-" + sector.ListaModalASP[i].EntityTypeCatalogOptionId).is(":checked");
                    var encontro = false;
                    for (var j = 0; j < sector.DataASP.length; j++) {
                        if (sector.ListaModalASP[i].EntityTypeCatalogOptionId == sector.DataASP[j].EntidadDestinoId) {
                            encontro = true;
                            if (!estado) {
                                sector.DataASP[j].Estado = 2;
                            }
                            else {
                                sector.DataASP[j].Estado = 0;
                            }
                        }
                    }
                    if (!encontro && estado) {
                        sector.DataASP.push({
                            Id: 0,
                            EntidadDestinoId: sector.ListaModalASP[i].EntityTypeCatalogOptionId,
                            EntidadDestinoAccion: sector.ListaModalASP[i].Nombre,
                            Estado: 0
                        });
                    }
                }
                vm.enabledSector(sector.SectorId, idEntidad, 'ASP');
            }
        }

        vm.GuardarConfiguracionEntidad = function (entidadId) {
            var entidad = vm.modelo.find(x => x.IdEntidad == entidadId);
            vm.NombreEntidad = entidad.NombreCompleto;
            vm.NombreSector = '';
            vm.NombrePasos = '';
            var error = false;
            if (entidad.UnidadesResponsables != null && entidad.UnidadesResponsables.length > 0) {
                var data = [];
                for (var x = 0; x < vm.modeloSectores.length; x++) {

                    var entidadSector = entidad.ConfiguracionMatriz.find(y => y.SectorId == vm.modeloSectores[x].SectorNegocioId);
                                        
                    var VRActuales = entidadSector.DataVR.filter(z => z.Estado == 0);
                    var VDActuales = entidadSector.DataVD.filter(z => z.Estado == 0);
                    var APActuales = entidadSector.DataAP.filter(z => z.Estado == 0);
                    var ASPActuales = entidadSector.DataASP.filter(z => z.Estado == 0);
                    var FEActuales = entidadSector.DataFE.filter(z => z.Estado == 0);

                    if ((VRActuales.length > 0 && VDActuales.length > 0 && APActuales.length > 0 && ASPActuales.length > 0 && FEActuales.length > 0) ||
                        (VRActuales.length == 0 && VDActuales.length == 0 && APActuales.length == 0 && ASPActuales.length == 0 && FEActuales.length == 0)) {

                        $('#errorColumnaMatriz-' + entidadId + "-" + vm.modeloSectores[x].SectorNegocioId).hide();

                        for (var f = 0; f < entidadSector.DataVR.length; f++) {
                            if (entidadSector.DataVR[f].Estado == 2 && entidadSector.DataVR[f].Id > 0) {
                                data.push({
                                    Id: entidadSector.DataVR[f].Id,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidVR,
                                    EntidadDestinoId: entidadSector.DataVR[f].EntidadDestinoId,
                                    Estado: 2
                                });
                            }
                            else if (entidadSector.DataVR[f].Estado == 0 && entidadSector.DataVR[f].Id == 0) {
                                data.push({
                                    Id: 0,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidVR,
                                    EntidadDestinoId: entidadSector.DataVR[f].EntidadDestinoId,
                                    Estado: 1
                                });
                            }
                        }

                        for (var f = 0; f < entidadSector.DataVD.length; f++) {
                            if (entidadSector.DataVD[f].Estado == 2 && entidadSector.DataVD[f].Id > 0) {
                                data.push({
                                    Id: entidadSector.DataVD[f].Id,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidVD,
                                    EntidadDestinoId: entidadSector.DataVD[f].EntidadDestinoId,
                                    Estado: 2
                                });
                            }
                            else if (entidadSector.DataVD[f].Estado == 0 && entidadSector.DataVD[f].Id == 0) {
                                data.push({
                                    Id: 0,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidVD,
                                    EntidadDestinoId: entidadSector.DataVD[f].EntidadDestinoId,
                                    Estado: 1
                                });
                            }
                        }

                        for (var f = 0; f < entidadSector.DataAP.length; f++) {
                            if (entidadSector.DataAP[f].Estado == 2 && entidadSector.DataAP[f].Id > 0) {
                                data.push({
                                    Id: entidadSector.DataAP[f].Id,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidAP,
                                    EntidadDestinoId: entidadSector.DataAP[f].EntidadDestinoId,
                                    Estado: 2
                                });
                            }
                            else if (entidadSector.DataAP[f].Estado == 0 && entidadSector.DataAP[f].Id == 0) {
                                data.push({
                                    Id: 0,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidAP,
                                    EntidadDestinoId: entidadSector.DataAP[f].EntidadDestinoId,
                                    Estado: 1
                                });
                            }
                        }

                        for (var f = 0; f < entidadSector.DataASP.length; f++) {
                            if (entidadSector.DataASP[f].Estado == 2 && entidadSector.DataASP[f].Id > 0) {
                                data.push({
                                    Id: entidadSector.DataASP[f].Id,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidASP,
                                    EntidadDestinoId: entidadSector.DataASP[f].EntidadDestinoId,
                                    Estado: 2
                                });
                            }
                            else if (entidadSector.DataASP[f].Estado == 0 && entidadSector.DataASP[f].Id == 0) {
                                data.push({
                                    Id: 0,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidASP,
                                    EntidadDestinoId: entidadSector.DataASP[f].EntidadDestinoId,
                                    Estado: 1
                                });
                            }
                        }

                        //Firma y Emisión
                        for (var f = 0; f < entidadSector.DataFE.length; f++) {
                            if (entidadSector.DataFE[f].Estado == 2 && entidadSector.DataFE[f].Id > 0) {
                                data.push({
                                    Id: entidadSector.DataFE[f].Id,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidFE,
                                    EntidadDestinoId: entidadSector.DataFE[f].EntidadDestinoId,
                                    Estado: 2
                                });
                            }
                            else if (entidadSector.DataFE[f].Estado == 0 && entidadSector.DataFE[f].Id == 0) {
                                data.push({
                                    Id: 0,
                                    CRTypeId: 102,
                                    EntidadResponsableId: entidad.EntityTypeCatalogOptionId,
                                    SectorId: vm.modeloSectores[x].SectorNegocioId,
                                    RolId: vm.rolGuidFE,
                                    EntidadDestinoId: entidadSector.DataFE[f].EntidadDestinoId,
                                    Estado: 1
                                });
                            }
                        }
                    }
                    else {
                        vm.NombreSector = vm.modeloSectores[x].Nombre;
                        
                        if (VRActuales.length == 0) vm.NombrePasos += ', Verificación de requisitos';
                        if (VDActuales.length == 0) vm.NombrePasos += ', Viabilidad definitiva';
                        if (APActuales.length == 0) vm.NombrePasos += ', Ajustes Proyecto';  
                        if (ASPActuales.length == 0) vm.NombrePasos += ', Asociar proyectos';  
                        if (FEActuales.length == 0) vm.NombrePasos += ', Firma y emisión';

                        error = true;
                    }

                    if (VRActuales.length == 0 || VDActuales.length == 0 || APActuales.length == 0 || ASPActuales.length == 0 || FEActuales.length == 0) {
                        $('#errorColumnaMatriz-' + entidadId + "-" + vm.modeloSectores[x].SectorNegocioId).show();
                        $('#errorTotalMatriz-' + entidadId).show();
                    }
                    else {
                        $('#errorTotalMatriz-' + entidadId).hide();
                    }
                }

                if (error) {
                    utilidades.mensajeError("Falta realizar la configuración de los siguientes pasos" + vm.NombrePasos +". Para el sector " + vm.NombreSector + " de la entidad " + vm.NombreEntidad, null, "Los datos no han podido guardarse.");
                    return false;
                }                
            }
            else {
                utilidades.mensajeError("No se han creado las unidades responsables de la entidad " + vm.NombreEntidad +", por favor diríjase a la sección 'Unidades Responsables'", null, "Los datos no han podido guardarse.");
                return false;
            }
            if (data.length > 0) {
                var datos = {
                    MatrizEntidadUnidad: data,
                    TipoFlujo: vm.tipoFlujoSeleccionado[entidadId]
                }
                servicioEntidades.actualizarMatrizEntidadDestino(datos)
                    .then(function (response) {
                        if (response.data.Exito) {
                            utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito.");
                            vm.ultimoIdEntidad = entidadId;
                            vm.nombreTabUtilizado = 'tab-flujo-';
                            vm.init();
                        }
                        else
                            utilidades.mensajeError(response.data.Mensaje, false);
                    });
            }
        }

        function abrirMensajeInformacionRegionalizacion() {
            utilidades.mensajeInformacionN("", null, null, "Aquí se configuran las secretarías, oficinas o dependencias de la entidad territorial responsables de la formulación y/o la viabilidad de los proyectos de inversión.");
        }

        function abrirMensajeConfiguracionFlujo() {
            utilidades.mensajeInformacionN("", null, null, "Aquí se indica que sectores por competencia tiene a cargo la unidad responsable y los pasos que podrá adelantar dentro de la viabilidad y ajuste de los proyectos");
        }

        vm.enabledRegister = function (idEntidad, estado) {
            if (estado == 0) {
                document.getElementById("inputNombre-" + idEntidad).value = '';
                $('#errorColumna-' + idEntidad).hide();
                $('#errorInput-' + idEntidad).hide();
                $('#Agregar-' + idEntidad).show();
                $('#AgregarD-' + idEntidad).hide();
                document.querySelectorAll('button[id=btnAdd-' + idEntidad + ']').forEach(element => {


                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnAddD-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('button[id=btnEdit-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnEditD-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('label[id=switchURD-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('label[id=switchUR-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('label[id=switchURDA-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnEditDA-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
            }
            else {
                $('#Agregar-' + idEntidad).hide();
                $('#AgregarD-' + idEntidad).show();
                document.querySelectorAll('button[id=btnAdd-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('button[id=btnAddD-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnEditD-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnEdit-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('label[id=switchUR-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('label[id=switchURD-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('label[id=switchURDA-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('button[id=btnEditDA-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
            }
            for (var i = 0; i < vm.modelo.length; i++) {
                if (vm.modelo[i].IdEntidad == idEntidad) {
                    vm.modelo[i].HabilitarRegistro = estado == 1 ? true : false;
                    i = vm.modelo.length;
                }  
            }
            
        }

        vm.saveRegister = function (item) {
            
            var NombreUnidad = document.getElementById("inputNombre-" + item.IdEntidad).value;
            if (NombreUnidad == '') {
                utilidades.mensajeError("Verifique los campos señalados", null, "Hay datos que presentan inconsistencias.");
                $('#errorColumna-' + item.IdEntidad).show();
                $('#errorInput-' + item.IdEntidad).show();
                return false;
            }

            var str = NombreUnidad.normalize('NFD')
                .replace(/([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi, "$1")
                .normalize();
            str = str.replace(/[^a-zA-Z0-9]/g, '');
            str = str.toLowerCase();
            let arr = item.NombreCompleto.split('-');
            var entidad = arr[0].replace(/ /g, "");
            entidad = entidad.toLowerCase();
            entidad = entidad.normalize('NFD')
                .replace(/([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi, "$1")
                .normalize();
            entidad = entidad.replace(/[^a-zA-Z0-9]/g, '');
            if (arr.length == 2) {
                var entidad1 = arr[1].replace(/ /g, "");
                entidad1 = entidad1.toLowerCase();
                entidad1 = entidad1.normalize('NFD')
                    .replace(/([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi, "$1")
                    .normalize();
                entidad1 = entidad1.replace(/[^a-zA-Z0-9]/g, '');
            }
            
            if (str.includes(entidad)) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener el nombre de la entidad.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (arr.length == 2) {
                if (str.includes(entidad1)) {
                    utilidades.mensajeError("La unidad responsable que desea crear no puede contener el nombre de la entidad.", null, "Hay datos que presentan inconsistencias.");
                    return false;
                }
            }
            if (str.includes("alcaldia")) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener la palabra reservada alcaldía.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (str.includes("gobernacion")) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener la palabra reservada gobernación.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (str.includes("municipio")) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener la palabra reservada municipio.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (str.includes("departamento")) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener la palabra reservada departamento.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (item.UnidadesResponsables != null) {
                for (var i = 0; i < item.UnidadesResponsables.length; i++) {
                    var strActual = item.UnidadesResponsables[i].Nombre.normalize('NFD')
                        .replace(/([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi, "$1")
                        .normalize();
                    strActual = strActual.replace(/[^a-zA-Z0-9]/g, '');
                    strActual = strActual.toLowerCase();
                    if (strActual == str) {
                        utilidades.mensajeError("El nombre de la unidad responsable que desea crear ya existe.", null, "Hay datos que presentan inconsistencias.");
                        return false;
                    }
                }
            }
            
            utilidades.mensajeWarning("Usted podrá editarla solo hasta antes de que la dependencia sea incluida en algún proceso. ¿Está seguro de continuar?", function funcionContinuar() {
                var objEntidad = {
                    Nombre : NombreUnidad,
                    TipoEntidad : 'UnidadResponsable',
                    IdSector : '733B8D20-C538-4C4E-BD58-D8C14908A535',
                    CabezaSector : false,
                    ParentGuid : item.IdEntidad,
                    IsActivo : true,
                    ParentId : item.EntityTypeCatalogOptionId,
                    RolPresupuesto: $("#checkboxEdit-" + item.IdEntidad).is(":checked")
                }
                servicioEntidades.guardarEntidad(objEntidad)
                    .then(function (response) {
                        if (response.data == null) {
                            utilidades.mensajeError("No fue posible almacenar el registro.", false);
                        }
                        else if (response.data.Exito) {
                            utilidades.mensajeSuccess("", false, false, false, "La dependencia '" + NombreUnidad + "' fue agregada con éxito.");
                            vm.ultimoIdEntidad = item.IdEntidad;
                            vm.init();
                        } else
                            utilidades.mensajeError(response.data.Mensaje, false);
                    });
                
            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, null, null, "La línea de información que será agregada no podrá eliminarse.");
        }

        vm.editRegister = function (idEntidad, idUnidadResponsable, activo, estado) {
            if (estado == 0) {
                vm.rowEditando[idUnidadResponsable] = false;
                $('#errorInput-' + idEntidad + "-" + idUnidadResponsable).hide();
                $('#errorColumna-' + idEntidad + "-" + idUnidadResponsable).hide();
                $('#Agregar-' + idEntidad).show();
                $('#AgregarD-' + idEntidad).hide();
                document.querySelectorAll('button[id=btnAdd-' + idEntidad + ']').forEach(element => {

                    
                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnAddD-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                
                
                document.querySelectorAll('label[id=switchURD-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('label[id=switchUR-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                
                if (activo) {
                    $('#divCheckboxEdit-' + idEntidad + "-" + idUnidadResponsable).hide();
                    $('#divCheckboxEditD-' + idEntidad + "-" + idUnidadResponsable).show();
                    $('#label-' + idEntidad + "-" + idUnidadResponsable).show();
                    $('#divInput-' + idEntidad + "-" + idUnidadResponsable).hide();
                    document.querySelectorAll('button[id=btnEdit-' + idEntidad + ']').forEach(element => {

                        element.removeAttribute("hidden")
                    });
                    document.querySelectorAll('button[id=btnEditD-' + idEntidad + ']').forEach(element => {

                        element.setAttribute("hidden", true);
                    });
                    $('#btnCancelEdit-' + idEntidad + "-" + idUnidadResponsable).hide();
                    $('#btnSaveEdit-' + idEntidad + "-" + idUnidadResponsable).hide();
                }
                
                document.querySelectorAll('label[id=switchURDA-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('button[id=btnEditDA-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
            }
            else {
                if (activo) {
                    $('#divCheckboxEditD-' + idEntidad + "-" + idUnidadResponsable).hide();
                    $('#divCheckboxEdit-' + idEntidad + "-" + idUnidadResponsable).show();
                    $('#label-' + idEntidad + "-" + idUnidadResponsable).hide();
                    $('#divInput-' + idEntidad + "-" + idUnidadResponsable).show();
                    $('#btnCancelEdit-' + idEntidad + "-" + idUnidadResponsable).show();
                    $('#btnSaveEdit-' + idEntidad + "-" + idUnidadResponsable).show();
                    document.querySelectorAll('button[id=btnEdit-' + idEntidad + ']').forEach(element => {

                        element.setAttribute("hidden", true);
                    });
                    document.querySelectorAll('button[id=btnEditD-' + idEntidad + ']').forEach(element => {

                        element.removeAttribute("hidden")
                    });
                }
                vm.rowEditando[idUnidadResponsable] = true;
                $('#Agregar-' + idEntidad).hide();
                $('#AgregarD-' + idEntidad).show();
                document.querySelectorAll('button[id=btnAdd-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden",true);
                });
                document.querySelectorAll('button[id=btnAddD-' + idEntidad+']').forEach(element => {
                    
                    element.removeAttribute("hidden")
                });
                
                
                document.querySelectorAll('label[id=switchUR-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('label[id=switchURD-' + idEntidad + ']').forEach(element => {

                    element.removeAttribute("hidden")
                });
                document.querySelectorAll('label[id=switchURDA-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });
                document.querySelectorAll('button[id=btnEditDA-' + idEntidad + ']').forEach(element => {

                    element.setAttribute("hidden", true);
                });                
            }
        }

        vm.editUnidadResponsable = function (item, total) {
            var NombreUnidad = document.getElementById("inputNombre-" + item.ParentGuid + "-" + item.UnidadResponsableId).value;
            if (NombreUnidad == '') {
                utilidades.mensajeError("Verifique los campos señalados",null,"Hay datos que presentan inconsistencias.");
                $('#errorColumna-' + item.ParentGuid + "-" + item.UnidadResponsableId).show();
                $('#errorInput-' + item.ParentGuid + "-" + item.UnidadResponsableId).show();
                return false;
            }
            var unidadNombreAnterior = "";
            var str = NombreUnidad.normalize('NFD')
                .replace(/([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi, "$1")
                .normalize();
            str = str.replace(/[^a-zA-Z0-9]/g, '');
            str = str.toLowerCase();
            let arr = total.NombreCompleto.split('-');
            var entidad = arr[0].replace(/ /g, "");
            entidad = entidad.toLowerCase();
            entidad = entidad.normalize('NFD')
                .replace(/([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi, "$1")
                .normalize();
            entidad = entidad.replace(/[^a-zA-Z0-9]/g, '');
            if (arr.length == 2) {
                var entidad1 = arr[1].replace(/ /g, "");
                entidad1 = entidad1.toLowerCase();
                entidad1 = entidad1.normalize('NFD')
                    .replace(/([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi, "$1")
                    .normalize();
                entidad1 = entidad1.replace(/[^a-zA-Z0-9]/g, '');
            }
            
            if (str.includes(entidad)) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener el nombre de la entidad.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (arr.length == 2) {
                if (str.includes(entidad1)) {
                    utilidades.mensajeError("La unidad responsable que desea crear no puede contener el nombre de la entidad.", null, "Hay datos que presentan inconsistencias.");
                    return false;
                }
            }
            if (str.includes("alcaldia")) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener la palabra reservada alcaldía.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (str.includes("gobernacion")) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener la palabra reservada gobernación.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (str.includes("municipio")) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener la palabra reservada municipio.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (str.includes("departamento")) {
                utilidades.mensajeError("La unidad responsable que desea crear no puede contener la palabra reservada departamento.", null, "Hay datos que presentan inconsistencias.");
                return false;
            }
            if (total.UnidadesResponsables != null) {
                for (var i = 0; i < total.UnidadesResponsables.length; i++) {
                    var strActual = total.UnidadesResponsables[i].Nombre.normalize('NFD')
                        .replace(/([^n\u0300-\u036f]|n(?!\u0303(?![\u0300-\u036f])))[\u0300-\u036f]+/gi, "$1")
                        .normalize();
                    strActual = strActual.replace(/[^a-zA-Z0-9]/g, '');
                    strActual = strActual.toLowerCase();
                    var strActual = total.UnidadesResponsables[i].Nombre.replace(/[^a-zA-Z0-9]/g, '');
                    strActual = strActual.toLowerCase();
                    if (item.UnidadResponsableId == total.UnidadesResponsables[i].UnidadResponsableId) {
                        unidadNombreAnterior = total.UnidadesResponsables[i].Nombre;
                    }
                    if (strActual == str && item.UnidadResponsableId != total.UnidadesResponsables[i].UnidadResponsableId) {
                        utilidades.mensajeError("El nombre de la unidad responsable que desea crear ya existe.", null, "Hay datos que presentan inconsistencias.");
                        return false;
                    }
                }
            }

            utilidades.mensajeWarning("Usted podrá editarla solo hasta antes de que la dependencia sea incluida en algún proceso. ¿Está seguro de continuar?", function funcionContinuar() {
                var objEntidad = {
                    Id: item.UnidadResponsableId,
                    Nombre: NombreUnidad,
                    TipoEntidad: 'UnidadResponsable',
                    IsActivo: $("#switchURV-" + item.ParentGuid + "-" + item.UnidadResponsableId).is(":checked"),
                    RolPresupuesto: $("#checkboxEdit-" + item.ParentGuid + "-" + item.UnidadResponsableId).is(":checked"),
                    EntityTypeCatalogOptionId: item.EntityTypeCatalogOptionId
                }
                vm.listValidarEntidad = [];
                vm.listValidarEntidad.push(item.EntityTypeCatalogOptionId)
                var dataValidar = {
                    ListaEntidades: vm.listValidarEntidad
                }
                servicioEntidades.validarFlujoConInstanciaActiva(dataValidar)
                    .then(function (response) {
                        if (!response.data.EstaActivo) {
                            servicioEntidades.ActualizarUnidadResponsable(objEntidad)
                                .then(function (response) {
                                    if (response.data.Exito) {
                                        utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito.");
                                        vm.ultimoIdEntidad = total.IdEntidad;
                                        vm.init();
                                    } else
                                        utilidades.mensajeError(response.data.Mensaje, false);
                                });
                        }
                        else {
                            vm.cancelEditEstadoUnidad(unidadNombreAnterior);
                        }                        
                    });                

            }, function funcionCancelar(reason) {
                console.log("reason", reason);
            }, null, null, "La línea de información que será agregada no podrá eliminarse.");
        }

        vm.editEstadoUnidad = function (item) {
            var estadoActual = $("#switchURV-" + item.ParentGuid + "-" + item.UnidadResponsableId).is(":checked");
            var mensajetitulo = '';
            var mensajebody = '';

            if (!estadoActual) {
                mensajetitulo = "La línea de la dependencia con '" + item.Nombre + "' será inactivada.";
                mensajebody = "Esto impedirá su visualización en otros espacios de Admin Banco. ¿Está seguro de continuar?";
            } 
            else {
                mensajetitulo = "La línea de la dependencia con '" + item.Nombre + "' será activada.";
                mensajebody = "¿Está seguro de continuar?";
            }
                
            utilidades.mensajeWarning(mensajebody, function funcionContinuar() {
                vm.listValidarEntidad = [];
                vm.listValidarEntidad.push(item.EntityTypeCatalogOptionId)
                var dataValidar = {
                    ListaEntidades: vm.listValidarEntidad
                }
                servicioEntidades.validarFlujoConInstanciaActiva(dataValidar)
                    .then(function (response) {
                        if (!response.data.EstaActivo) {
                            var objEntidad = {
                                Id: item.UnidadResponsableId,
                                Nombre: item.Nombre,
                                TipoEntidad: 'UnidadResponsable',
                                IsActivo: estadoActual,
                                RolPresupuesto: item.RolPresupuesto
                            }
                            servicioEntidades.ActualizarUnidadResponsable(objEntidad)
                                .then(function (response) {
                                    if (response.data.Exito) {
                                        if (!objEntidad.IsActivo)
                                            utilidades.mensajeSuccess("", false, false, false, "Los línea de la dependencia con '" + item.Nombre + "' ha sido inactivada con éxito.");
                                        else
                                            utilidades.mensajeSuccess("", false, false, false, "Los línea de la dependencia con '" + item.Nombre + "' ha sido activado con éxito.");
                                        vm.ultimoIdEntidad = item.ParentGuid;
                                        vm.init();
                                    } else
                                        utilidades.mensajeError(response.data.Mensaje, false);
                                });
                        }
                        else {
                            vm.cancelEditEstadoUnidad(item.Nombre);
                        }
                    });
            }, function funcionCancelar(reason) {
                if ($("#switchURV-" + item.ParentGuid + "-" + item.UnidadResponsableId).is(":checked")) {
                    $("#switchURV-" + item.ParentGuid + "-" + item.UnidadResponsableId).prop('checked', false);
                }
                else {
                    $("#switchURV-" + item.ParentGuid + "-" + item.UnidadResponsableId).prop('checked', true);
                }

                console.log("reason", reason);
            }, null, null, mensajetitulo);
        }

        vm.cancelEditEstadoUnidad = function (dependencia) {
            setTimeout(function () {
                utilidades.mensajeError("La dependencia 'Secretaría '" + dependencia +"' no se puede editar, esta asociada a un proceso activo.", false);
            }, 500);
        }

        vm.cancelarConsultaUsuario = function (item) {
            vm.editarConfiguracionUsuario[item.IdEntidad] = [];
            vm.resultadosUsuariosBusqueda[item.IdEntidad] = [];
            vm.listEntidadesConsulta = [];
            vm.editarConfiguracionUsuarioRow[item.IdEntidad] = false;
            vm.sinResultadosUsuario[item.IdEntidad] = false;
            vm.resultadosUsuario[item.IdEntidad] = false;
            vm.buscarIdentificacion[item.IdEntidad] = '';
            vm.buscarNombre[item.IdEntidad] = '';
        }

        vm.consultarUsuariosConfiguracion = function (item) {
            if ((vm.buscarIdentificacion[item.IdEntidad] == '' || vm.buscarIdentificacion[item.IdEntidad] == undefined) &&
                (vm.buscarNombre[item.IdEntidad] == '' || vm.buscarNombre[item.IdEntidad] == undefined)) {
                vm.sinResultadosUsuario[item.IdEntidad] = true;
                vm.resultadosUsuario[item.IdEntidad] = false;
                return false;
            }
            vm.editarConfiguracionUsuario[item.IdEntidad] = [];
            vm.resultadosUsuariosBusqueda[item.IdEntidad] = [];
            vm.editarConfiguracionUsuarioRow[item.IdEntidad] = false;
            vm.listEntidadesConsulta = [];
            vm.listEntidadesConsulta.push(item.IdEntidad);

            if (item.UnidadesResponsables != null) {
                for (var i = 0; i < item.UnidadesResponsables.length; i++) {
                    if (item.UnidadesResponsables[i].IsActivo)
                        vm.listEntidadesConsulta.push(item.UnidadesResponsables[i].UnidadResponsableId);
                }
            }            

            var filtro = {
                Identificacion: vm.buscarIdentificacion[item.IdEntidad] == undefined ? '' : vm.buscarIdentificacion[item.IdEntidad],
                Nombre: vm.buscarNombre[item.IdEntidad] == undefined ? '' : vm.buscarNombre[item.IdEntidad], 
                Entidades: vm.listEntidadesConsulta
            }
            servicioEntidades.obtenerUsuariosPorNombreIdentificacion(filtro)
                .then(function (response) {
                    if (response.statusText === "OK" || response.status === 200) {
                        if (response.data.length == 0) {
                            vm.sinResultadosUsuario[item.IdEntidad] = true;
                            vm.resultadosUsuario[item.IdEntidad] = false;
                            vm.resultadosUsuarioContador[item.IdEntidad] = response.data.length;
                        }
                        else {
                            vm.sinResultadosUsuario[item.IdEntidad] = false;
                            vm.resultadosUsuario[item.IdEntidad] = true;
                            vm.resultadosUsuarioContador[item.IdEntidad] = response.data.length;
                            vm.resultadosUsuariosBusqueda[item.IdEntidad] = response.data;
                        }
                    }
                });
        }

        vm.editarDetalleUsuario = function (item, itemUsuario) {
            vm.editarConfiguracionUsuario[item.IdEntidad] = [];
            vm.nombreUsuarioEditar[item.IdEntidad] = itemUsuario.NombreUsuario;
            vm.tipoUsuarioEditar[item.IdEntidad] = itemUsuario.TipoIdentificacion;
            vm.identificacionUsuarioEditar[item.IdEntidad] = itemUsuario.Identificacion;
            vm.usuarioDNPEditar[item.IdEntidad] = itemUsuario.IdUsuarioDNP;
            vm.idUsuarioSeleccionado[item.IdEntidad] = itemUsuario;
            vm.listEntidadesConsulta = [];
            vm.listEntidadesConsulta.push(item.IdEntidad);

            if (item.UnidadesResponsables != null) {
                for (var i = 0; i < item.UnidadesResponsables.length; i++) {
                    if (item.UnidadesResponsables[i].IsActivo)
                        vm.listEntidadesConsulta.push(item.UnidadesResponsables[i].UnidadResponsableId);
                }
            }
            

            var filtro = {
                IdUsuario: itemUsuario.IdUsuario,
                Entidades: vm.listEntidadesConsulta
            }
            servicioEntidades.obtenerSectoresPorUsuarioEntidad(filtro)
                .then(function (response) {
                    if (response.statusText === "OK" || response.status === 200) {
                        vm.resultadosUsuario[item.IdEntidad] = false;
                        vm.editarConfiguracionUsuario[item.IdEntidad] = response.data;
                        vm.editarConfiguracionUsuarioRow[item.IdEntidad] = true;
                        
                        
                    }
                });
        }

        vm.AjustarSectoresUsuario = function (item) {
            var data = vm.editarConfiguracionUsuario[item.IdEntidad];
            if (vm.estadoParaEditarUsuario[item.IdEntidad] == 0) {
                for (var i = 0; i < data.length; i++) {
                    for (var j = 0; j < data[i].UsuarioSectores.length; j++) {
                        $('#divCheckboxSector1EditD-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector1).hide();
                        $('#divCheckboxSector1Edit-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector1).show();
                        $('#divCheckboxSector2EditD-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector2).hide();
                        $('#divCheckboxSector2Edit-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector2).show();
                        $('#divCheckboxSector3EditD-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector3).hide();
                        $('#divCheckboxSector3Edit-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector3).show();
                    }

                }
                vm.estadoParaEditarUsuario[item.IdEntidad] = 1;
                vm.rowEditarUsuario[item.IdEntidad] = true;
                $("#GuardarUsuario-" + item.IdEntidad).attr('disabled', false);
            }
            else if (vm.estadoParaEditarUsuario[item.IdEntidad] == 1) {
                utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {
                    for (var i = 0; i < data.length; i++) {
                        for (var j = 0; j < data[i].UsuarioSectores.length; j++) {
                            $('#divCheckboxSector1Edit-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector1).hide();
                            $('#divCheckboxSector1EditD-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector1).show();
                            $('#divCheckboxSector2Edit-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector2).hide();
                            $('#divCheckboxSector2EditD-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector2).show();
                            $('#divCheckboxSector3Edit-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector3).hide();
                            $('#divCheckboxSector3EditD-' + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector3).show();
                        }

                    }
                    vm.estadoParaEditarUsuario[item.IdEntidad] = 0;
                    vm.rowEditarUsuario[item.IdEntidad] = false;
                    $("#GuardarUsuario-" + item.IdEntidad).attr('disabled', true);
                    vm.editarDetalleUsuario(item, vm.idUsuarioSeleccionado[item.IdEntidad]);
                    utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelado con éxito.");

                }, function funcionCancelar(reason) {
                    console.log("reason", reason);
                }, null, null, "Los datos que posiblemente haya diligenciado se perderán.");
                
            }        
        }

        vm.GuardarConfiguracionUsuario = function (item) {
            var data = vm.editarConfiguracionUsuario[item.IdEntidad];
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].UsuarioSectores.length; j++) {
                    data[i].UsuarioSectores[j].EstadoSector1 = $("#checkboxSector1Edit-" + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector1).is(":checked");
                    data[i].UsuarioSectores[j].EstadoSector2 = $("#checkboxSector2Edit-" + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector2).is(":checked");
                    data[i].UsuarioSectores[j].EstadoSector3 = $("#checkboxSector3Edit-" + item.IdEntidad + "-" + data[i].IdEntidad + "-" + data[i].IdRol + "-" + data[i].UsuarioSectores[j].IdSector3).is(":checked");
                }
            }
            servicioEntidades.guardarSectoresPorUsuarioEntidad(data)
                .then(function (response) {
                    if (response.statusText === "OK" || response.status === 200) {
                        vm.estadoParaEditarUsuario[item.IdEntidad] = 0;
                        vm.rowEditarUsuario[item.IdEntidad] = false;
                        vm.editarDetalleUsuario(item, vm.idUsuarioSeleccionado[item.IdEntidad]);
                        utilidades.mensajeSuccess("", false, false, false, "Los datos fueron guardados con éxito.");
                    }
                });
        }

        vm.validarRolPresupuesto = function (entidadId) {
            var entidad = vm.modelo.find(x => x.IdEntidad == entidadId);
            if (entidad == null || entidad == '' || entidad == undefined)
                return false;
            if (entidad.UnidadesResponsables != null && entidad.UnidadesResponsables != undefined) {
                var rolPresupuestoEntidad = entidad.UnidadesResponsables.find(x => x.RolPresupuesto == true);
                if (rolPresupuestoEntidad == null || rolPresupuestoEntidad == '' || rolPresupuestoEntidad == undefined) {
                    utilidades.mensajeError("Al menos una unidad responsable debe estar marcada como 'Asume rol de presupuesto'.", false);
                    return false;
                }
            }
            return false;
            
        }        
    };
    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone.entidades').controller('unidadResponsableController', unidadResponsableController);
})();