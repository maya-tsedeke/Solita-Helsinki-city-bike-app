import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup,Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import ValidateForm from 'src/app/helper/validateform';
import { AuthService } from 'src/app/Services/auth.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit{
  type:string="password";
  isText:boolean=false;
  eyeIcon:string="fa-eye-slash";
  loginForm!:FormGroup;
  constructor(
    private fb:FormBuilder,
    private auth:AuthService,
    private router:Router,
    private toast:NgToastService

    ){}//Inject
  ngOnInit(): void {
    //initialize form and group
    this.loginForm=this.fb.group({
      username: ['',Validators.required],
      password: ['',Validators.required]
    })
  }
  hideShowPass(){
    this.isText=!this.isText;
    this.isText?this.eyeIcon="fa-eye":this.eyeIcon="fa-eye-slash";
    this.isText?this.type="text":this.type="password";
  }
onLogin(){
  if(this.loginForm.valid)
  {
    console.log(this.loginForm.value);
    this.auth.login(this.loginForm.value)
    .subscribe({
      next:(res)=>{
        console.log(res.message);
        this.loginForm.reset();
        this.auth.storeToken(res.token);
        
        this.toast.success({detail:"SUCCESS",summary:res.message,duration:5000})
        
        this.router.navigate(['dashboard']);
        
      },
      error:(err)=>{
        this.toast.error({detail:"ERROR",summary:"Somthing went wrong",duration:5000})
        console.log(err);
      }
    })
  }
  else{
   ValidateForm.validateAllFormFields(this.loginForm);

   this.toast.error({detail:"ERROR",summary:"Your form is Invalid",duration:5000})
  }
}

}
