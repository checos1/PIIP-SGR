(function () {
	'use strict';

	modalAgregarLocalizacionController.$inject = [
		'$sessionStorage',
		'$uibModalInstance',
		'localizacionJustificacionServicio',
		'utilidades',
		'$filter'
	];

	function modalAgregarLocalizacionController(
		$uibModalInstance,
		$sessionStorage,
		localizacionJustificacionServicio,
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
		vm.proyectoId = $uibModalInstance.InstanciaSeleccionada.ProyectoId;
		vm.BPIN = $uibModalInstance.idObjetoNegocio;
		var lstLocalizacion = angular.fromJson($uibModalInstance.listaObjLocalizacion).NuevaLocalizacion;
		if (lstLocalizacion != null) {
			var nuevaLocalizacion = lstLocalizacion.filter(x => x.LocalizacionId == $uibModalInstance.LocalizacionId);
		}
		var lstRolesTodo = $uibModalInstance.usuario.roles;
		var arreglolistaEntidadesDepartamentos = [];
		var listaAgrupaciones = [];
		var listaTipoAgrupacion = [];
		vm.listaLocalizacionNueva=[];
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
			vm.model = {
				modulos: {
					administracion: false,
					backbone: true
				}
			}
			obtenerDepartamentos();
			obtenerTipoAgrupacion();

		}
		vm.paramJustificacion = {
			ProyectoId: 0,
			SeccionCapituloId: 0,
			InstanciaId: $uibModalInstance.idInstancia,
			Justificacion: ''


		};

		function obtenerDepartamentos() {
			return localizacionJustificacionServicio.obtenerDepartamentos()
				.then(respuesta => {
					if (!respuesta.data)
						return;

					vm.listaDepartamento = respuesta.data;

					if (nuevaLocalizacion != null && $uibModalInstance.LocalizacionId != 0) {
						vm.Departamento = vm.listaDepartamento.filter(x => x.Id == nuevaLocalizacion[0].DepartamentoId)[0];
						cambioDepartamento();
					}

				})
				.catch(error => {
					console.log(error);
					toastr.error("Hubo un error al cargar los Departamentos");
				});
		}

		function obtenerTipoAgrupacion() {
			listaTipoAgrupacion = [];
			return localizacionJustificacionServicio.obtenerTipoAgrupaciones(parametros)
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
					if (nuevaLocalizacion != null && $uibModalInstance.LocalizacionId != 0) {
						vm.tipoAgrupacion = vm.listaTipoAgrupaciones.filter(x => x.Id == nuevaLocalizacion[0].TipoAgrupacionId)[0];
						obtenerAgrupaciones()
					}
				})
				.catch(error => {
					console.log(error);
					toastr.error("Hubo un error al cargar la lista de Tipo Agrupaciones");
				});
		}

		function obtenerAgrupaciones() {
			listaAgrupaciones = [];
			return localizacionJustificacionServicio.obtenerAgrupacionesCompleta()
				.then(respuesta => {
					if (!respuesta.data)
						return;

					var arregloAgrupaciones = respuesta.data;
					arregloAgrupaciones = arregloAgrupaciones.filter(x => x.TipoAgrupacionId == vm.tipoAgrupacion.Id && x.MunicipalityId == vm.Municipio.Id);

					vm.listaAgrupaciones = arregloAgrupaciones;
					if (nuevaLocalizacion != null && $uibModalInstance.LocalizacionId != 0) {
						vm.Agrupaciones = vm.listaAgrupaciones.filter(x => x.Id == nuevaLocalizacion[0].AgrupacionId)[0];
					}
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
			return localizacionJustificacionServicio.obtenerMunicipios(parametros, deptoid)
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
					if (nuevaLocalizacion != null && $uibModalInstance.LocalizacionId != 0) {
						vm.Municipio = vm.listaMunicipios.filter(x => x.Id == nuevaLocalizacion[0].MunicipioId)[0];
					}
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
				if (vm.Departamento == '' || vm.Departamento == undefined) {
					utilidades.mensajeError('Debe seleccionar un Departamento.');
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
					if (objJson.Localizacion != null && objJson.Localizacion != undefined) {
						var validadionLocalizacion = objJson.Localizacion.filter(x => x.DepartamentoId == depId && x.MunicipioId == munId && x.TipoAgrupacionId == tipAgrId && x.AgrupacionId == agrId)
						if (validadionLocalizacion.length > 0) {
							utilidades.mensajeError('La localización ya existe.');
							return false;
						}
					}
					if (objJson.NuevaLocalizacion != null && objJson.NuevaLocalizacion != undefined) {
						var validadionNUevaLocalizacion = objJson.NuevaLocalizacion.filter(x => x.DepartamentoId == depId && x.MunicipioId == munId && x.TipoAgrupacionId == tipAgrId && x.AgrupacionId == agrId)

						if (validadionNUevaLocalizacion.length > 0) {
							utilidades.mensajeError('La localización ya existe.');
							return false;
						}

					}
					if (tipAgrId != null && agrId == null) {
						utilidades.mensajeError('Debe seleccionar una agrupación');
						return false;
					}

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
					var seccionCapituloId = document.getElementById("id-capitulo-datosgeneraleslocalizaciones");
					var parametro = {
						ProyectoId: $uibModalInstance.InstanciaSeleccionada.ProyectoId,
						BPIN: $uibModalInstance.idObjetoNegocio,
						Accion: "Insert",
						Justificacion: document.getElementById("Justificacionlocalizacion1").value,
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
						localizacionJustificacionServicio.guardarLocalizacion(parametro, usuarioDNP).then(function (response) {
							if (response.statusText === "OK" || response.status === 200) {
								parent.postMessage("cerrarModal", window.location.origin);
								utilidades.mensajeSuccess("Se adiciono la linea de información: " + depName + " /" + munName + " /" + tipAgrName + " / " + agrName + " en la parte inferior de la tabla ''localizaciones''. Recuerde justificar esta modificación en el campo justificación", false, false, false, "Los datos fueron agregados y guardados con éxito!");

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
						ProyectoId: $uibModalInstance.InstanciaSeleccionada.ProyectoId,
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
					localizacionJustificacionServicio.guardarLocalizacion(parametro, usuarioDNP).then(function (response) {
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
	}

	angular.module('backbone').controller('modalAgregarLocalizacionController', modalAgregarLocalizacionController);

})();
