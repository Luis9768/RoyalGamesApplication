import Pagination from '@mui/material/Pagination';

interface PaginacaoProps {
  count: number;
  page: number;
  onChange: (event: React.ChangeEvent<unknown>, value: number) => void;
}

const Paginacao = ({ count, page, onChange }: PaginacaoProps) => {
  return (
    <div style={{ display: 'flex', justifyContent: 'center', marginTop: '40px', alignItems: 'center'}}>
      <Pagination 
        count={count} // Quantidade total de páginas
        page={page} // Página atual
        onChange={onChange} // Função que roda ao mudar de página
        variant="outlined"
        shape="rounded"
        sx={{
          '& .MuiPaginationItem-root': {
            color: '#ffffff',
            borderColor: '#ff2759',
            fontSize: '1rem',
            fontWeight: 'bold',
            '&:hover': {
              backgroundColor: 'rgba(255, 39, 89, 0.2)',
            },
          },
          '& .Mui-selected': {
            backgroundColor: '#ff2759 !important',
            color: '#ffffff',
            borderColor: '#ff2759',
          },
          '& .MuiPaginationItem-icon': {
            fill: '#ff2759', 
          }
        }}
      />
    </div>
  );
};

export default Paginacao;