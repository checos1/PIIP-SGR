(function () {
	'use strict';

    modalAgregarLocalizacionRegionalizacionController.$inject = [
		'$sessionStorage',
		'$uibModalInstance',
		'comunesServicio',
		'utilidades',
		'$filter'
	];

    function modalAgregarLocalizacionRegionalizacionController(
		$uibModalInstance,
		$sessionStorage,
        comunesServicio,
		utilidades,
		$filter
	) {
		var vm = this;
		vm.init = init;
		vm.cerrar = $sessionStorage.close;
		vm.guardar;
		vm.actualizar;
		vm.municipioVisible = false;
		vm.nombreComponente = "localizacionJustificacion";
		vm.cambioDepartamento = cambioDepartamento;
		vm.cambioMunicipio = cambioMunicipio;
		vm.obtenerAgrupaciones = obtenerAgrupaciones;
		vm.proyectoId = $uibModalInstance.proyectoId;
        vm.BPIN = $uibModalInstance.proyectoId;
		var lstLocalizacion = angular.fromJson($uibModalInstance.listaObjLocalizacion).NuevaLocalizacion;
		var lstRolesTodo = $uibModalInstance.usuario.roles;
		var listaAgrupaciones = [];
		var listaTipoAgrupacion = [];
        vm.listaLocalizacionNueva = [];        
        vm.localizacionSeleccionada = 1; 
        vm.localizacionTipo = 1;
        vm.selectedTipo = selectedTipo;        
		var lsRoles = [];
		for (var ls = 0; ls < lstRolesTodo.length; ls++)
			lsRoles.push(lstRolesTodo[ls].IdRol)

		var parametros = {
			"Aplicacion": nombreAplicacionBackbone,
			"ListaIdsRoles": lsRoles,
			"IdUsuario": usuarioDNP,
			"IdObjeto": $uibModalInstance.idInstancia,      //'88ea329d-f240-4868-9df7-86c74fb2ecfa',
			"InstanciaId": $uibModalInstance.idInstancia,   //'88ea329d-f240-4868-9df7-86c74fb2ecfa', 
			"IdFiltro": $uibModalInstance.idAccionAnterior
		}

        function init() {		
            vm.localizacionTipo = 1; 
            selectedTipo(1);
			obtenerDepartamentos();
			obtenerTipoAgrupacion();
		}
		
		function obtenerDepartamentos() {
            return comunesServicio.obtenerDepartamentos()
				.then(respuesta => {
					if (!respuesta.data)
						return;
					vm.listaDepartamento = respuesta.data;				
				})
				.catch(error => {
					console.log(error);
					toastr.error("Hubo un error al cargar los departamentos a localizar en la regionalización.");
				});
		}

		function obtenerTipoAgrupacion() {
			listaTipoAgrupacion = [];
            return comunesServicio.obtenerTipoAgrupaciones(parametros)
				.then(respuesta => {
					if (!respuesta.data)
						return;

					var arregloTipoAgrupaciones = jQuery.parseJSON(respuesta.data);
					for (var ls = 0; ls < arregloTipoAgrupaciones.length; ls++) {
						var tipoAgrupacion = {
							"Name": arregloTipoAgrupaciones[ls].Name,
							"Id": arregloTipoAgrupaciones[ls].Id,
							"Bpin": arregloTipoAgrupaciones[ls].Bpin,
						}
						listaTipoAgrupacion.push(tipoAgrupacion);
					}
					vm.listaTipoAgrupaciones = listaTipoAgrupacion;					
                    obtenerAgrupaciones()
				})
				.catch(error => {
					console.log(error);
					toastr.error("Hubo un error al cargar la lista de Tipo Agrupaciones");
				});
		}

		function obtenerAgrupaciones() {
			listaAgrupaciones = [];
            return comunesServicio.obtenerAgrupacionesCompleta()
				.then(respuesta => {
					if (!respuesta.data)
						return;
					var arregloAgrupaciones = respuesta.data;
                    if (vm.Municipio != undefined) {
                        if (vm.Municipio.Id != undefined && vm.tipoAgrupacion.Id != undefined) {
                            arregloAgrupaciones = arregloAgrupaciones.filter(x => x.TipoAgrupacionId == vm.tipoAgrupacion.Id && x.MunicipalityId == vm.Municipio.Id);
                        }                    
                    }                    
					vm.listaAgrupaciones = arregloAgrupaciones;					
				})
				.catch(error => {
					console.log(error);
					toastr.error("Hubo un error al cargar la lista de Agrupaciones");
				});
		}

		function cambioMunicipio() {
			vm.tipoAgrupacion = null;
			vm.Agrupaciones = null;
		}

		function cambioDepartamento() {
			var deptoid = vm.Departamento.Id;
			var listaMunicipios = [];
            return comunesServicio.obtenerMunicipios(parametros, deptoid)
				.then(respuesta => {
					if (!respuesta.data)
						return;

					var arregloMunicipios = jQuery.parseJSON(respuesta.data);
					for (var ls = 0; ls < arregloMunicipios.CatalogosRelacionados.length; ls++) {
						var municipios = {
							"Name": arregloMunicipios.CatalogosRelacionados[ls].Name,
							"Id": arregloMunicipios.CatalogosRelacionados[ls].Id
						}
						listaMunicipios.push(municipios);
					}
					vm.listaMunicipios = listaMunicipios;					
				})
				.catch(error => {
					console.log(error);
					toastr.error("Hubo un error al cargar los Municipios");
				});
		}

		vm.guardar = function () {
			if ($uibModalInstance.LocalizacionId != 0) {
				vm.actualizar();
			}
			else {               
                if ((vm.Departamento == '' || vm.Departamento == undefined) && vm.localizacionTipo == 3) {                    
                  utilidades.mensajeError('Debe seleccionar un Departamento.');
                  return false;                    
				}
				else {
                    if ((vm.Departamento != undefined ) && (vm.localizacionTipo == 1 || vm.localizacionTipo == 2)) {
                        utilidades.mensajeError('Para localización Nacional o Por regionalizar no aplica la selección de Departamento.');
                        return false;
                    }
                    else {
                    var objJson = angular.fromJson($uibModalInstance.listaObjLocalizacion);
					var munId = null;
					var depId = null;
					var agrId = null;
					var tipAgrId = null;
					var munName = null;
					var depName = null;
					var agrName = null;
					var tipAgrName = null;
					if (vm.Municipio != undefined) {
						munId = vm.Municipio.Id;
						munName = vm.Municipio.Name;
					}
					if (vm.Departamento != undefined) {
						depId = vm.Departamento.Id;
						depName = vm.Departamento.Name;
					}
					if (vm.tipoAgrupacion != undefined) {
						tipAgrId = vm.tipoAgrupacion.Id;
						tipAgrName = vm.tipoAgrupacion.Name;
					}
					if (vm.Agrupaciones != undefined) {
						agrId = vm.Agrupaciones.Id;
						agrName = vm.Agrupaciones.Name;
					}
					var validadionLocalizacion = objJson.Localizacion.filter(x => x.DepartamentoId == depId && x.MunicipioId == munId && x.TipoAgrupacionId == tipAgrId && x.AgrupacionId == agrId)

				        if (objJson.NuevaLocalizacion !== null && objJson.NuevaLocalizacion != undefined) {
							var validadionNUevaLocalizacion = objJson.NuevaLocalizacion.filter(x => x.DepartamentoId == depId && x.MunicipioId == munId && x.TipoAgrupacionId == tipAgrId && x.AgrupacionId == agrId)
						}

                        if (vm.localizacionTipo == 1) {
                            var validarNacional = objJson.Localizacion.filter(x => x.Region == "Nacional")
                            if (validarNacional.length > 0) {
                                utilidades.mensajeError('La localización Nacional ya existe.');
                                return false;
                            }
                        }
                        if (vm.localizacionTipo == 2) {
                            var validarNacional = objJson.Localizacion.filter(x => x.Region == "Por Regionalizar")
                            if (validarNacional.length > 0) {
                                utilidades.mensajeError('La localización Por Regionalizar ya existe.');
                                return false;
                            }
                        }
                        if (validadionLocalizacion.length > 0) {
                        if (vm.localizacionTipo == 3) {
                            utilidades.mensajeError('La localización ya existe.');
                            return false;
                        }                        
					}


					if (validadionNUevaLocalizacion != undefined && validadionNUevaLocalizacion != null)  {
							if (validadionNUevaLocalizacion.length > 0) {
								if (vm.localizacionTipo == 3) {
									utilidades.mensajeError('La localización ya existe.');
									return false;
								}
							}

						}


					if (tipAgrId != null && agrId == null) {
						utilidades.mensajeError('Debe seleccionar una agrupación');
						return false;
					}
                    let localizacion = [];
                    let l = [];
                    if (vm.localizacionTipo == 1)
                    {
                        l = {
                            LocalizacionId: $uibModalInstance.LocalizacionId,
                            RegionId: 1,
                            Region: null,
                            DepartamentoId: 34,
                            Departamento: depName,
                            MunicipioId: munId,
                            Municipio: munName,
                            TipoAgrupacionId: tipAgrId,
                            TipoAgrupacion: tipAgrName,
                            AgrupacionId: agrId,
                            Agrupacion: agrName,
                        };
                    }
                    if (vm.localizacionTipo == 2) {
                        l = {
                            LocalizacionId: $uibModalInstance.LocalizacionId,
                            RegionId: 10,
                            Region: null,
                            DepartamentoId: depId,
                            Departamento: depName,
                            MunicipioId: munId,
                            Municipio: munName,
                            TipoAgrupacionId: tipAgrId,
                            TipoAgrupacion: tipAgrName,
                            AgrupacionId: agrId,
                            Agrupacion: agrName,
                        };
                    }
                    if (vm.localizacionTipo == 3) {
                        l = {
                            LocalizacionId: $uibModalInstance.LocalizacionId,
                            RegionId: vm.Departamento.RegionId,
                            Region: null,
                            DepartamentoId: depId,
                            Departamento: depName,
                            MunicipioId: munId,
                            Municipio: munName,
                            TipoAgrupacionId: tipAgrId,
                            TipoAgrupacion: tipAgrName,
                            AgrupacionId: agrId,
                            Agrupacion: agrName,
                        };
                    }                   
					localizacion.push(l);
					var seccionCapituloId = document.getElementById("id-capitulo-datosgeneraleslocalizaciones");
					var parametro = {
                        ProyectoId: $uibModalInstance.proyectoId,                        
						BPIN: $uibModalInstance.idObjetoNegocio,
						Accion: "Insert",
						Justificacion: "Programacion",
						SeccionCapituloId: seccionCapituloId != undefined ? seccionCapituloId.innerHTML : 0,
						InstanciaId: $uibModalInstance.idInstancia,
						NuevaLocalizacion: localizacion,
					};

					if (depName == null) {
						depName = "N/A";
					}
					if (munName == null) {
						munName = "N/A";
					}
					if (tipAgrName == null) {
						tipAgrName = "N/A";
					}
					if (agrName == null) {
						agrName = "N/A";
					}
					utilidades.mensajeWarning("Se va a agregar una localización", function funcionContinuar() {
                        comunesServicio.guardarLocalizacion(parametro, usuarioDNP).then(function (response) {
							if (response.statusText === "OK" || response.status === 200) {
								parent.postMessage("cerrarModal", window.location.origin);
                                if (vm.localizacionTipo == 3) {
                                    utilidades.mensajeSuccess("Se adicionó la línea de Otra localización: " + depName + " /" + munName + " /" + tipAgrName + " / " + agrName + ". ", false, false, false, "Los datos fueron agregados y guardados con éxito!");
                                }
                                else {
                                    if (vm.localizacionTipo == 1) {
                                        utilidades.mensajeSuccess("Si la localización Nacional ya existe para el proyecto, no se adicionará una nueva localización.", false, false, false, "Los datos de localización Nacional fueron guardados con éxito!");
                                    }
                                    else {
                                        utilidades.mensajeSuccess("Si la localización Por Regionalizar ya existe para el proyecto, no se adicionará una nueva localización.", false, false, false, "Los datos de localización Por Regionalizar fueron guardados con éxito!");
                                    }
                                }                               
								vm.cerrar();
							} else {
								swal('', "Error al realizar la operación", 'error');
							}
						});

					}, function funcionCancelar(reason) {
						console.log("reason", reason);
					});
                  }
                }
			}
        }

		vm.actualizar = function () {

			var objJson = angular.fromJson($uibModalInstance.listaObjLocalizacion);
			var munId = null;
			var depId = null;
			var agrId = null;
			var tipAgrId = null;
			var munName = null;
			var depName = null;
			var agrName = null;
			var tipAgrName = null;

			if (vm.Municipio != undefined) {
				munId = vm.Municipio.Id;
				munName = vm.Municipio.Name;
			}
			if (vm.Departamento != undefined) {
				depId = vm.Departamento.Id;
				depName = vm.Departamento.Name;
			}
			if (vm.tipoAgrupacion != undefined) {
				tipAgrId = vm.tipoAgrupacion.Id;
				tipAgrName = vm.tipoAgrupacion.Name;
			}
			if (vm.Agrupaciones != undefined) {
				agrId = vm.Agrupaciones.Id;
				agrName = vm.Agrupaciones.Name;
			}

			var validadionLocalizacion = objJson.Localizacion.filter(x => x.DepartamentoId == depId && x.MunicipioId == munId && x.TipoAgrupacionId == tipAgrId && x.AgrupacionId == agrId)
			var validadionNUevaLocalizacion = objJson.NuevaLocalizacion.filter(x => x.DepartamentoId == depId && x.MunicipioId == munId && x.TipoAgrupacionId == tipAgrId && x.AgrupacionId == agrId)

			if (validadionLocalizacion.length > 0) {
				utilidades.mensajeError('La localización ya existe.');
				return false;
			}

			if (validadionNUevaLocalizacion.length > 0) {
				utilidades.mensajeError('La localización ya existe.');
				return false;
			}

			if (vm.Departamento == '' || vm.Departamento == undefined) {
				utilidades.mensajeError('Debe seleccionar un Departamento.');
				return false;
			}
			else {
				
				if ($uibModalInstance.LocalizacionId != 0) {

					let localizacion = [];
					let l = {
						LocalizacionId: $uibModalInstance.LocalizacionId,
						RegionId: vm.Departamento.RegionId,
						Region: null,
						DepartamentoId: depId,
						Departamento: depName,
						MunicipioId: munId,
						Municipio: munName,
						TipoAgrupacionId: tipAgrId,
						TipoAgrupacion: tipAgrName,
						AgrupacionId: agrId,
						Agrupacion: agrName,
					};
					localizacion.push(l);

					var parametro = {
                        ProyectoId: $uibModalInstance.ProyectoId,                        
						BPIN: $uibModalInstance.idObjetoNegocio,
						Accion: "Update",
						NuevaLocalizacion: localizacion,
					};
				}

				if (depName == null) {
					depName = "N/A";
				}
				if (munName == null) {
					munName = "N/A";
				}
				if (tipAgrName == null) {
					tipAgrName = "N/A";
				}
				if (agrName == null) {
					agrName = "N/A";
				}
				utilidades.mensajeWarning("Se va a editar una localización", function funcionContinuar() {
                    comunesServicio.guardarLocalizacion(parametro, usuarioDNP).then(function (response) {
						if (response.data && (response.statusText === "OK" || response.status === 200)) {
							if (response.data.Exito) {
								parent.postMessage("cerrarModal", window.location.origin);
								utilidades.mensajeSuccess("Se actualiza la linea de información: " + depName + " /" + munName + " /" + tipAgrName + " / " + agrName + " en la parte inferior de la tabla ''localizaciones''. Recuerde Justificar esta modificación en el campo Justificación", false, false, false, "Los datos fueron editados con éxito!");
								vm.cerrar();
							} else {
								swal('', response.data.Mensaje, 'warning');
							}
						} else {
							swal('', "Error al realizar la operación", 'error');
						}
					});

				}, function funcionCancelar(reason) {
					console.log("reason", reason);
				});
			}
		}

        function selectedTipo(localizacionTipo) {
            vm.localizacionSeleccionada = localizacionTipo           
        }
    }
  
    angular.module('backbone').controller('modalAgregarLocalizacionRegionalizacionController', modalAgregarLocalizacionRegionalizacionController);

})();
