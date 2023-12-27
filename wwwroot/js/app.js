let password = document.getElementById("password");
let confirmPass = document.getElementById("confirmpassword");
let togglePassword = document.getElementById("toggle");
let toggleConfirmPass = document.getElementById("toggleconfirm");
var icon = togglePassword.getElementsByTagName("i");
var iconConfirm = toggleConfirmPass.getElementsByTagName("i");

function showHidePass() {
  if (password.type == "password") {
    password.setAttribute("type", "text");
    icon[0].classList.remove("fa-eye-slash");
    icon[0].classList.add("fa-eye");
  } else {
    password.setAttribute("type", "password");
    icon[0].classList.remove("fa-eye");
    icon[0].classList.add("fa-eye-slash");
  }
}
function showHideConfirmPass() {
  if (confirmPass.type == "password") {
    confirmPass.setAttribute("type", "text");
    iconConfirm[0].classList.remove("fa-eye-slash");
    iconConfirm[0].classList.add("fa-eye");
  } else {
    confirmPass.setAttribute("type", "password");
    iconConfirm[0].classList.remove("fa-eye");
    iconConfirm[0].classList.add("fa-eye-slash");
  }
}
