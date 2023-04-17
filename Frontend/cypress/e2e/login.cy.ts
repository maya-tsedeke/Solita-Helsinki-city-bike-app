describe('Login Page', () => {
  it('Login Sucess', () => {
    cy.visit('/');
    cy.url().should('include','login');
    cy.get('#username').type('admin').should('have.value','admin');
    cy.get('#password').type('1819@Parul').should('have.value','1819@Parul');
    cy.get('.btn').click();
    cy.get('#classError').should('have.text','Please fill all fields');
    cy.get('#required').should('have.text','This field is Required');
  })
})