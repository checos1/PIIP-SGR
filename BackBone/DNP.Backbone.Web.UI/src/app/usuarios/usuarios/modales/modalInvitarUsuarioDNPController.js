(function () {
	'use strict';

	modalInvitarUsuarioDNPController.$inject = [
		'objInvitarUsuario',
		'actions',
		'$uibModalInstance',
		'utilidades',
		'servicioUsuarios'
	];

	function modalInvitarUsuarioDNPController(
		objInvitarUsuario,
		actions,
		$uibModalInstance,
		utilidades,
		servicioUsuarios
	) {
		var vm = this;

		vm.init = init;
		vm.cerrar = $uibModalInstance.dismiss;

		function init() {
			vm.tiposIdentificacion = objInvitarUsuario.tiposIdentificacion;
			vm.listaEntidades = objInvitarUsuario.listaEntidades;
			vm.listaPerfilesBackbone = objInvitarUsuario.listaPerfilesBackbone;
			vm.cambiaTipoValor = cambiaTipoValor;
			vm.esCorreoValido = false;
			vm.esCorreoInstitucionalValido = false;
			vm.btnBuscarActivo = false;
			vm.btnInvitarActivo = false;
			vm.mostrarFormulario = false;
			vm.mostrarMensaje1 = false;
			vm.mostrarMensaje2 = false;
			vm.mostrarMensaje3 = false;
			vm.BanderaDivPerfiles = false;
			vm.BanderaDivPerfiles2 = false;
			vm.mostrarFormularioPerfilamiento = false;
			vm.esTipoIdentificacionValido = false;
			vm.esIdentificacionValido = false;
			vm.esNombresValido = false;
			vm.esApellidosValido = false;
			vm.esEntidadValido = false;
			vm.esPerfilValido = false;
			vm.encontroUsuarioPIIP = false;
			vm.encontroUsuarioDirectorio = false;
			vm.esFormularioValido = false;
			vm.activarFormulario = false;
			vm.activarFormularioPerfilamiento = false;
			vm.BanderaInputnumeros = false;
			vm.tiposUsuario = [
				{ id: 1, name: 'Usuario DNP' },
				{ id: 2, name: 'Usuario externo' }
			];
			vm.tipoUsuario = 0;
			vm.model = {
				correoInstitucional: '',
				tipoIdentificacion: '',
				identificacion: '',
				nombres: '',
				apellidos: '',
				idEntidad: '',
				idPerfilBackbone: ''
			}

			vm.usuarioPIIP = {
				IdUsuario: '',
				IdUsuarioDnp: '',
				TipoIdentificacion: '',
				Identificacion: '',
				Nombre: ''
			}

			vm.usuarioDirectorio = {
				objectId: '',
				mail: '',
				mailNickname: '',
				displayName: '',
				givenName: '',
				surname: '',
				userPrincipalName: ''
			}
		}
		function cambiaTipoValor(tipoValor) {
			if (tipoValor) {
				switch (tipoValor) {
					case 1:
						actions.abrirModalInvitarUsuarioDNP()
						break;
					case 2:
						actions.abrirModalInvitarUsuarioExterno()
						break;
				}
				vm.cerrar();
			}

		}
		vm.validarCorreoInstitucional = function () {
			vm.mostrarFormulario = false;
			vm.btnBuscarActivo = false;
			vm.esCorreoInstitucionalValido = false;

			vm.limpiarFormulario();

			let esCorreoValido = utilidades.validarEmail(correoInstitucional.value);
			let esCorreoInstitucional = correoInstitucional.value.toUpperCase().includes("DNP.GOV.CO");

			if (esCorreoValido && esCorreoInstitucional) {
				vm.esCorreoInstitucionalValido = true;
				vm.btnBuscarActivo = true;
			}
			else {
				vm.btnBuscarActivo = false;
			}
		}

		vm.validarTipoIdentificacion = function () {
			vm.esTipoIdentificacionValido = false;

			let esCampoValido = utilidades.isNotNull(vm.model.tipoIdentificacion);

			if (esCampoValido) {
				vm.esTipoIdentificacionValido = true

				if (vm.mostrarMensaje1 == true) {
					vm.model.identificacion = '';
				}
				if (vm.model.tipoIdentificacion == "CC" || vm.model.tipoIdentificacion == "NI") {
					vm.BanderaInputnumeros = true;
				}
				else {
					vm.BanderaInputnumeros = false;
				}
				vm.validarIdentificacion();




			}
			vm.validarFormulario();
		}

		vm.validarIdentificacion = function () {
			vm.esIdentificacionValido = false;

			let esCampoValido = utilidades.isNotNull(vm.model.identificacion);

			if (esCampoValido) {
				vm.esIdentificacionValido = true
			}
			vm.validarFormulario();
		}

		vm.validarNombres = function () {
			vm.esNombresValido = false;

			let esCampoValido = utilidades.isNotNull(vm.model.nombres);

			if (esCampoValido) {
				vm.esNombresValido = true
			}
			vm.validarFormulario();
		}

		vm.validarApellidos = function () {
			vm.esApellidosValido = false;

			let esCampoValido = utilidades.isNotNull(vm.model.apellidos);

			if (esCampoValido) {
				vm.esApellidosValido = true
			}
			vm.validarFormulario();
		}

		vm.validarEntidad = function () {
			vm.esEntidadValido = false;

			let esCampoValido = utilidades.isNotNull(vm.model.idEntidad);

			if (esCampoValido) {
				vm.esEntidadValido = true
				let entidad = vm.obtenerEntidad(vm.model.idEntidad);
				vm.obtenerListadoPerfilesXEntidad(entidad.PadreId);

			}
			else {
				vm.BanderaDivPerfiles2 = false;
			}

			vm.validarFormulario();
		}

		vm.validarPerfil = function () {
			vm.esPerfilValido = false;

			//let esCampoValido = utilidades.isNotNull(vm.model.idPerfilBackbone);

			if (vm.ListaPerfilesUsuario.length > 0) {
				vm.esPerfilValido = true
			}
			vm.validarFormulario();
		}

		vm.validarFormulario = function () {

			vm.esFormularioValido = false;
			if (
				vm.esCorreoInstitucionalValido
				&& vm.esTipoIdentificacionValido
				&& vm.esIdentificacionValido
				&& vm.esNombresValido
				&& vm.esApellidosValido
				&& vm.esEntidadValido
				&& vm.esPerfilValido
			) {
				vm.esFormularioValido = true;
			}
		}

		vm.activarBotonInvitar = function () {
			return vm.esFormularioValido && vm.btnInvitarActivo && (vm.activarFormulario || vm.activarFormularioPerfilamiento);
		}

		vm.limpiarFormulario = function () {
			vm.model.tipoIdentificacion = '';
			vm.model.identificacion = '';
			vm.model.nombres = '';
			vm.model.apellidos = '';
			vm.model.idEntidad = '';
			vm.model.idPerfilBackbone = '';
		}

		vm.buscarUsuario = function () {
			vm.encontroUsuarioPIIP = false;
			vm.encontroUsuarioDirectorio = false;

			vm.limpiarFormulario();
			vm.validarFormulario();

			vm.buscarUsuarioPIIP();
		}
		vm.MostrarDivPerfilesUsuario = function (idbandera) {
			if (idbandera == 1) {
				vm.BanderaDivPerfiles = true;
				vm.btnBuscarActivo = false;
				vm.btnInvitarActivo = false;
			}
			else {
				vm.BanderaDivPerfiles = false;
				vm.btnBuscarActivo = true;
				vm.btnInvitarActivo = true;
			}
		}

		vm.buscarUsuarioPIIP = function () {
			vm.mostrarMensaje1 = false;
			let correo = vm.model.correoInstitucional;
			servicioUsuarios.autorizacionObtenerUsuarioPorCorreoDNP(correo).then(function (response) {
				if (response.data) {
					const data = response.data;

					vm.encontroUsuarioPIIP = utilidades.isNotNull(data.IdUsuarioDnp)

					if (vm.encontroUsuarioPIIP) {
						vm.usuarioPIIP.IdUsuario = data.IdUsuario ? data.IdUsuario : ''
						vm.usuarioPIIP.IdUsuarioDnp = data.IdUsuarioDnp ? data.IdUsuarioDnp : ''
						vm.usuarioPIIP.TipoIdentificacion = data.TipoIdentificacion ? data.TipoIdentificacion : ''
						vm.usuarioPIIP.Identificacion = data.Identificacion ? data.Identificacion : ''
						vm.usuarioPIIP.Nombre = data.Nombre ? data.Nombre : ''

						let conformacionUsuario = vm.usuarioPIIP.Nombre.split(":");

						vm.model.tipoIdentificacion = vm.usuarioPIIP.TipoIdentificacion
						vm.model.identificacion = vm.usuarioPIIP.Identificacion
						vm.model.nombres = conformacionUsuario[0];
						if (conformacionUsuario[1]) vm.model.apellidos = conformacionUsuario[1];

						vm.validarTipoIdentificacion();
						vm.validarIdentificacion();
						vm.validarNombres();
						vm.validarApellidos();
						vm.validarPerfil();

					}
					else {
						vm.usuarioPIIP.IdUsuario = '';

					}

				}
				else {
					vm.usuarioPIIP.IdUsuario = '';

				}

				vm.validarFormulario();
				vm.buscarUsuarioDirectorio();
				vm.BanderaDivPerfiles = false;
				vm.BanderaDivPerfiles2 = false;
				vm.ListaPerfilesUsuario = [];

			}, function (error) {
				let mensaje = "Hubo un error en el servicio de autorizacion para obtener los datos del usuario";
				toastr.error(mensaje);
			});

		}

		vm.buscarUsuarioDirectorio = function () {

			servicioUsuarios.identidadObtenerUsuarioPorId(correoInstitucional.value).then(function (response) {
				if (response.data) {
					const data = response.data;

					vm.encontroUsuarioDirectorio = utilidades.isNotNull(data.mail)

					if (vm.encontroUsuarioDirectorio) {
						vm.usuarioDirectorio.objectId = data.objectId ? data.objectId : ''
						vm.usuarioDirectorio.mail = data.mail ? data.mail : ''
						vm.usuarioDirectorio.mailNickname = data.mailNickname ? data.mailNickname : ''
						vm.usuarioDirectorio.displayName = data.displayName ? data.displayName : ''
						vm.usuarioDirectorio.givenName = data.givenName ? data.givenName : ''
						vm.usuarioDirectorio.surname = data.surname ? data.surname : ''
						vm.usuarioDirectorio.userPrincipalName = data.userPrincipalName ? data.userPrincipalName : ''

						vm.model.nombres = vm.usuarioDirectorio.givenName
						vm.model.apellidos = vm.usuarioDirectorio.surname

						vm.validarNombres();
						vm.validarApellidos();
						vm.validarEntidad();
						vm.validarFormulario();

					}
				}

				vm.calcularMensajeBusqueda();

			}, function (error) {
				console.log(error);
				toastr.error("Hubo un error al cargar las informaciones del usuario");
			});

		}

		vm.calcularMensajeBusqueda = function () {
			vm.mostrarFormulario = false;
			vm.mostrarFormularioPerfilamiento = false;
			vm.activarFormulario = false;
			vm.activarFormularioPerfilamiento = false;
			vm.btnInvitarActivo = false;
			vm.mostrarMensaje1 = false;
			vm.mostrarMensaje2 = false;
			vm.mostrarMensaje3 = false;

			// Si existe en la PIIP y en el Directorio activo (Debe activar el formulario de perfilamiento)
			if (vm.encontroUsuarioPIIP && vm.encontroUsuarioDirectorio) {
				vm.mostrarFormulario = true;
				vm.mostrarFormularioPerfilamiento = true;
				vm.activarFormularioPerfilamiento = true;

				if (vm.model.tipoIdentificacion == '' || vm.model.identificacion == '') {
					vm.activarFormulario = true;
				}

				vm.btnInvitarActivo = true;

				vm.validarFormulario();
				vm.mostrarMensaje3 = true;
				//utilidades.mensajeSuccess(
				//    "Para gestionar la información del usuario, puede hacerlo desde la opción de administración de usuarios.",
				//    false,
				//    null,
				//    null,
				//    "El usuario ya existe en la PIIP"
				//);
				return;
			}

			// Si no existe en la PIIP y en el Directorio activo
			if (!vm.encontroUsuarioPIIP && vm.encontroUsuarioDirectorio) {
				vm.mostrarFormulario = true;
				vm.mostrarFormularioPerfilamiento = true;
				vm.activarFormulario = true;
				vm.activarFormularioPerfilamiento = true;
				vm.btnInvitarActivo = true;

				vm.validarFormulario();
				vm.mostrarMensaje1 = true;
				//utilidades.mensajeSuccess(
				//    "Para invitar al usuario al PIIP, completar el diligenciamiento del siguiente formulario.",
				//    false,
				//    null,
				//    null,
				//    "Usuario existente en el directorio del DNP"
				//);
				return;
			}

			// Si no existe en el Directorio activo
			if (!vm.encontroUsuarioDirectorio) {
				vm.mostrarMensaje2 = true;
				//utilidades.mensajeSuccess(
				//    "El usuario no se encuentra creado en el directorio del DNP. Por favor contactar al administrador del directorio.",
				//    false,
				//    null,
				//    null,
				//    "Usuario no existente en el directorio del DNP"
				//);
				return;
			}

		}


		vm.calcularMensajeBusqueda = function () {
			vm.mostrarFormulario = false;
			vm.mostrarFormularioPerfilamiento = false;
			vm.activarFormulario = false;
			vm.activarFormularioPerfilamiento = false;
			vm.btnInvitarActivo = false;
			vm.mostrarMensaje1 = false;
			vm.mostrarMensaje2 = false;
			vm.mostrarMensaje3 = false;

			// Si existe en la PIIP y en el Directorio activo (Debe activar el formulario de perfilamiento)
			if (vm.encontroUsuarioPIIP && vm.encontroUsuarioDirectorio) {
				vm.mostrarFormulario = true;
				vm.mostrarFormularioPerfilamiento = true;
				vm.activarFormularioPerfilamiento = true;

				if (vm.model.tipoIdentificacion == '' || vm.model.identificacion == '') {
					vm.activarFormulario = true;
				}

				vm.btnInvitarActivo = true;

				vm.validarFormulario();
				vm.mostrarMensaje3 = true;
				//utilidades.mensajeSuccess(
				//    "Para gestionar la información del usuario, puede hacerlo desde la opción de administración de usuarios.",
				//    false,
				//    null,
				//    null,
				//    "El usuario ya existe en la PIIP"
				//);
				return;
			}

			// Si no existe en la PIIP y en el Directorio activo
			if (!vm.encontroUsuarioPIIP && vm.encontroUsuarioDirectorio) {
				vm.mostrarFormulario = true;
				vm.mostrarFormularioPerfilamiento = true;
				vm.activarFormulario = true;
				vm.activarFormularioPerfilamiento = true;
				vm.btnInvitarActivo = true;

				vm.validarFormulario();
				vm.mostrarMensaje1 = true;
				//utilidades.mensajeSuccess(
				//    "Para invitar al usuario al PIIP, completar el diligenciamiento del siguiente formulario.",
				//    false,
				//    null,
				//    null,
				//    "Usuario existente en el directorio del DNP"
				//);
				return;
			}

			// Si no existe en el Directorio activo
			if (!vm.encontroUsuarioDirectorio) {
				vm.mostrarMensaje2 = true;
				//utilidades.mensajeSuccess(
				//    "El usuario no se encuentra creado en el directorio del DNP. Por favor contactar al administrador del directorio.",
				//    false,
				//    null,
				//    null,
				//    "Usuario no existente en el directorio del DNP"
				//);
				return;
			}

		}

		vm.obtenerListadoPerfilesXEntidad = function (idEntidad) {
			var promise = actions.obtenerListadoPerfilesXEntidad(idEntidad);
			promise.then(function (response) {
				vm.listaPerfilesBackbone = response.data;
				if (vm.listaPerfilesBackbone.length > 0) {

					let IdUsuario = vm.usuarioPIIP.IdUsuario;
					let IdEntidadUsuario = vm.model.idEntidad;

					if (IdUsuario != '') {
						vm.obtenerListadoPerfilesXEntidadYUsuario(IdEntidadUsuario, IdUsuario);
					}

					vm.BanderaDivPerfiles2 = true;
				}
			}, function (error) {
				console.log("error", error);
			});
		}
		vm.ListaPerfilesUsuario = [];
		vm.ValidarPerfilyaregistra = function (IdPerfil) {

			//var rueba = vm.listaPerfilesUsuarioBackbone.filter(x => x.Id == IdPerfil);
			//if (rueba.length > 0) {
			//    return true            
			//}
			if (vm.ListaPerfilesUsuario.indexOf(IdPerfil) === -1) {
				return false
			} else if (vm.ListaPerfilesUsuario.indexOf(IdPerfil) > -1) {
				return true
			}


		}
		vm.toggleCurrency = function (IdPerfil) {
			if ($('#chk-' + IdPerfil).is(':checked')) {
				vm.ListaPerfilesUsuario.push(IdPerfil);
			} else {
				var toDel = vm.ListaPerfilesUsuario.indexOf(IdPerfil);
				vm.ListaPerfilesUsuario.splice(toDel, 1);
			}
			vm.validarPerfil();
		};
		vm.obtenerListadoPerfilesXEntidadYUsuario = function (idEntidad, idUsuario) {
			var promise = actions.obtenerListadoPerfilesXEntidadYUsuario(idEntidad, idUsuario);
			promise.then(function (response) {
				var prueba = response.data;
				vm.listaPerfilesUsuarioBackbone = prueba;//prueba.filter(x => x.IdEntidad == idEntidad)[0].Perfiles;
				vm.ListaPerfilesUsuario = [];
				vm.listaPerfilesUsuarioBackbone.forEach(function (item, i) {
					var banderaid = vm.listaPerfilesBackbone.filter(x => x.IdPerfil == item.IdPerfil)[0];
					if (banderaid != undefined) {
						vm.ListaPerfilesUsuario.push(item.IdPerfil);
					}

				});
				vm.validarPerfil();
			}, function (error) {
				console.log("error", error);
			});
		}

		vm.obtenerEntidad = function (id) {
			return vm.listaEntidades.filter(x => x.Id == id)[0];
		}

		vm.obtenerPerfil = function (id) {
			/*     return vm.listaPerfilesBackbone.filter(x => x.IdPerfil == id)[0];*/
			var NombresPerfil = '';
			if (vm.listaPerfilesBackbone.length > 0) {

				vm.ListaPerfilesUsuario.forEach(function (item, i) {
					NombresPerfil = NombresPerfil + vm.listaPerfilesBackbone.filter(x => x.IdPerfil == item)[0].NombrePerfil + ', ';
				});
			}
			return NombresPerfil == '' ? 'Ninguno' : NombresPerfil;
		}

		vm.invitarUsuario = function () {

			let modelo = {
				tipoIdentificacion: vm.model.tipoIdentificacion,
				identificacion: vm.model.identificacion,
				correo: vm.model.correoInstitucional,
				nombre: vm.model.nombres,
				apellido: vm.model.apellidos,
				idEntidad: vm.model.idEntidad,
				nombreEntidad: vm.obtenerEntidad(vm.model.idEntidad).Nombre,
				idPerfilBackbone: vm.ListaPerfilesUsuario,
				nombrePerfil: vm.obtenerPerfil(vm.ListaPerfilesUsuario),
				IdUsuarioDNP: vm.model.tipoIdentificacion + vm.model.identificacion,
				tieneModuloAdministracion: false,
				tieneModuloBackbone: true,
				tipoInvitacion: 1
			};

			console.log('invitarUsuario DNP: ', modelo);

			actions.registrarUsuarioPIIP(modelo);

			vm.activarFormulario = false;
			vm.activarFormularioPerfilamiento = false;
			vm.btnInvitarActivo = false;
			vm.BanderaDivPerfiles2 = false;
			vm.BanderaDivPerfiles = false;

		}

	}

	angular.module('backbone.usuarios').controller('modalInvitarUsuarioDNPController', modalInvitarUsuarioDNPController);
})();
