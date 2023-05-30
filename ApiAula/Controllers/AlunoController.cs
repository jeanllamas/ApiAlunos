using ApiAula.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAula.Controllers
{
    [ApiController]

    [Route("api/aula/aluno")]

    public class AlunoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlunoController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Cadastro
        [HttpPost]
        public async Task<ActionResult<int>> Post(Aluno aluno)
        {
            string[] nome = aluno.Nome.Split(' ');
            
            if (String.IsNullOrEmpty(nome[0]) || nome[0].Length < 2)                
                return BadRequest("Nome inválido");

            if (String.IsNullOrEmpty(nome[1]) || nome[1].Length < 2)
                return BadRequest("Sobrenome inválido");

            if (Double.IsNaN(aluno.Nota) || aluno.Nota < 0)
                return BadRequest("Nota inválida");

            if (String.IsNullOrEmpty(Convert.ToString(aluno.NumeroFaltas)) || aluno.NumeroFaltas < 0)
                return BadRequest("Quantidade de faltas inválida");

            if (aluno.Nota >= 6 && aluno.NumeroFaltas < 5)
                aluno.Situacao = "Aprovado";
            else
                aluno.Situacao = "Reprovado";

            //Exemplo verificação de e-mail
            /*if (!aluno.Email.Contains("@")))
                return BadRequest("E-mail inválido!");*/

            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return aluno.Id;
        }

        //Leitura de um Elmento do banco dados pelo ID
        [HttpGet]
        public async Task<ActionResult<List<Aluno>>> GetAlunos(int id)
        {
            List<Aluno> listaAlunos;
            listaAlunos = await _context.Alunos.ToListAsync();
            return listaAlunos;
        }

        //Filtrar aluno pela primeira letra do nome
        [HttpGet("alfabeto/{strBegin}")]
        public async Task<ActionResult<List<Aluno>>> GetByAlfabeto(string strBegin)
        {
            List<Aluno> listaAlunos;
            listaAlunos = await _context.Alunos.AsQueryable().Where(a => a.Nome.StartsWith(strBegin)).ToListAsync();
            return listaAlunos;
        }

        [HttpGet("notas/{doubleBegin}")]
        public async Task<ActionResult<List<Aluno>>> GetByNota(string doubleBegin)
        {
            List<Aluno> listaAlunos;

            string[] nota = doubleBegin.Split(' ');

            if (nota[0] == ">")
                listaAlunos = await _context.Alunos.AsQueryable().Where(a => a.Nota > Convert.ToDouble(nota[1])).ToListAsync();
            else if (nota[0] == "<")
                listaAlunos = await _context.Alunos.AsQueryable().Where(a => a.Nota < Convert.ToDouble(nota[1])).ToListAsync();
            else
                return BadRequest("Insira apenas > ou < e separe o símbolo do número por espaço.");
            return listaAlunos;

        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            Aluno aluno;
            aluno = await _context.Alunos.FirstOrDefaultAsync(p => p.Id == id);

            //Se nao for encontrado -> sinaliza elemento nao encontrado
            if (aluno == null)
                return NotFound();

            //se for remove o elemento
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Atualizar Conteudo no SGBD
        [HttpPut]
        public async Task<ActionResult> Atualizar(Aluno aluno)
        {
            string[] nome = aluno.Nome.Split(' ');

            if (String.IsNullOrEmpty(nome[0]) || nome[0].Length < 2)
                return BadRequest("Nome inválido");

            if (String.IsNullOrEmpty(nome[1]) || nome[1].Length < 2)
                return BadRequest("Sobrenome inválido");

            if (Double.IsNaN(aluno.Nota) || aluno.Nota < 0)
                return BadRequest("Nota inválida");

            if (String.IsNullOrEmpty(Convert.ToString(aluno.NumeroFaltas)) || aluno.NumeroFaltas < 0)
                return BadRequest("Quantidade de faltas inválida");

            if (aluno.Nota >= 6 && aluno.NumeroFaltas < 5)
                aluno.Situacao = "Aprovado";
            else
                aluno.Situacao = "Reprovado";

            _context.Attach(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
