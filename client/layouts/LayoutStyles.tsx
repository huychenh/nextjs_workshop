import { makeStyles } from '@mui/styles'

const useStyles = makeStyles({
  root: {
    flexGrow: 1,
  },
  navbar: {
    backgroundColor: '#203040',
    '& a': {
      color: '#ffffff',
      marginLeft: 10,
    },
  },
  title: {
    flexGrow: 1,
  },
  menu: {
    paddingRight: 20,
  },
  loginButton: {
    paddingLeft: 20,
    borderLeft: 'solid 2px #ffffff',
  },
  main: {
    minHeight: '80vh',
  },
  footer: {
    textAlign: 'center',
  },
})

export default useStyles
