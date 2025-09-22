#[Instrucciones en Español para Grupo A y Grupo B](#instrucciones-para-utilizar-esta-plantilla).

#[Instructions in English for Group I](#instructions-to-use-this-template).

# Instrucciones para utilizar esta plantilla.

## 1. Cree la organización 

Cree la organización para su equipo con el nombre **ISII2526GrupoXYourName**, donde GrupoX puede ser GrupoA o GrupoB siguiendo las instrucciones disponibles en:  
[Create an organization](https://docs.github.com/en/organizations/collaborating-with-groups-in-organizations/creating-a-new-organization-from-scratch).
- Establezca que dicha organización pertenece a su **cuenta personal**. 

## 2. Clone el repositorio

Cree el repositorio de su equipo con el nombre **ISII2526TeamName** utilizando esta plantilla
- Siga las instrucciones disponibles en:  
[Create a repository from a template](https://docs.github.com/es/repositories/creating-and-managing-repositories/creating-a-repository-from-a-template#creating-a-repository-from-a-template).
- Estableca que el **propietario (owner) es la organización que acaba de crear**.
- Estableca la visibilidad (visibility) **como pública**.

## 3. Configure el repositorio de su equipo.

**3.1. En la rama development modificar el archivo `info.yml`.**

```yaml
project:
  name: 'ISII-2526-GrupoX-Team'
  owner: 'GrupoX'
  teamId: 'Team'
  identities: {}
  notifications:
    email: 'member1@alu.uclm.es,member2@alu.uclm.es,member3@alu.uclm.es,member4@alu.uclm.es'
  members:
    member1:
      name: 'Name1'
      surname: 'Surname1' 
      githubUsername: 'Name1gitUserName1'
    member2:
      name: 'Name2'
      surname: 'Surname2' 
      githubUsername: 'Name1gitUserName2'
    member3:
      name: 'Name3'
      surname: 'Surname3' 
      githubUsername: 'Name1gitUserName3'
    member4:
      name: 'Name4'
      surname: 'Surname4' 
      githubUsername: 'Name1gitUserName4'
```

- Donde, GrupoX puede ser GrupoA, GrupoB, o GrupoI, y Team es el nombre del equipo
    > Por ejemplo, el equipo "TheLeaders" matriculado en el GrupoI sería:
    >```yaml
    >name: 'ISII-2526-GrupoI-TheLeaders'
    >owner: 'GrupoI'
    >teamId: 'TheLeaders'
    >```

- Sustituir los datos de cada memberN por los del miembro real.
- Si el número de miembros es 3 entonces, eliminar member4 de la sección `members`.
- Modificar la cadena de `notifications.email` para que contengan sólo los correos @alu.uclm.es de todos los miembros separados por comas y sin espacios.

**3.2. Añada los miembros de su equipo al repo.**

Añada con el role **Write**  a los miembros de su equipo siguiendo las instrucciones disponibles en:  
[Managing teams and people with access to your repository](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/managing-repository-settings/managing-teams-and-people-with-access-to-your-repository).



# Instructions to use this template.

## 1. Create the organisation

Create your team's organisation with the name **ISII2526GroupIYourName**:
- Follow the instructions available at 
[Create an organisation](https://docs.github.com/en/organizations/collaborating-with-groups-in-organizations/creating-a-new-organization-from-scratch).
- Establish that this organisation **belongs to your personal account**


## 2. Clone the repository

Create your team's repository with the name **ISII2526TeamName**, using this template:
- Follow the instructions available at:  
[Create a repository from a template](https://docs.github.com/es/repositories/creating-and-managing-repositories/creating-a-repository-from-a-template#creating-a-repository-from-a-template).
- Establish that **the owner is the organisation you have just created.**
- Establish that **Visibility is Public.**


## 3. Configure your team's repository.

**3.1. In the development branch, modify the `info.yml` file.**

```yaml
project:
  name: 'ISII-2526-GrupoX-Team'
  owner: 'GrupoX'
  teamId: 'Team'
  identities: {}
  notifications:
    email: 'member1@alu.uclm.es,member2@alu.uclm.es,member3@alu.uclm.es,member4@alu.uclm.es'
  members:
    member1:
      name: 'Name1'
      surname: 'Surname1' 
      githubUsername: 'Name1gitUserName1'
    member2:
      name: 'Name2'
      surname: 'Surname2' 
      githubUsername: 'Name1gitUserName2'
    member3:
      name: 'Name3'
      surname: 'Surname3' 
      githubUsername: 'Name1gitUserName3'
    member4:
      name: 'Name4'
      surname: 'Surname4' 
      githubUsername: 'Name1gitUserName4'
```

- Where GroupX must be GroupI, and Team is the name of the team
    > For example, the team ‘TheLeaders’ registered in GroupI would be:
    >```yaml
    >name: 'ISII-2526-GrupoI-TheLeaders'
    >owner: 'GroupI'
    >teamId: 'TheLeaders'
    >```
- Replace the data for each memberN with that of the actual member.
- If the number of members is 3, delete member4 from the `members` section.
- Modify the `notifications:email` string so that it only contains the @alu.uclm.es email addresses of all members, separated by commas and without spaces.

**3.2. Add your team members to the team.**

Add members with the role **Write** to your repo by following the instructions available at:  
[Managing teams and people with access to your repository](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/managing-repository-settings/managing-teams-and-people-with-access-to-your-repository).
