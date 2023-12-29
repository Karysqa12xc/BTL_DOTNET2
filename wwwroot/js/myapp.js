let password = document.getElementById("password");
let confirmPass = document.getElementById("confirmpassword");
let toggleConfirmPass = document.getElementById("toggleconfirm");
let togglePassword = document.getElementById("toggle");
var icon = togglePassword.getElementsByTagName("i");
var iconConfirm = toggleConfirmPass.getElementsByTagName("i");

function chooseFile(fileInput) {
  if (fileInput.files && fileInput.files[0]) {
    var reader = new FileReader();
    reader.onload = (e) => {
      $("#avatar").attr("src", e.target.result);
    };
    reader.readAsDataURL(fileInput.files[0]);
  }
}

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
