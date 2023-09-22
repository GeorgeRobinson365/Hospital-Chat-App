const projects = [
  {
    id: 0,
    img: '/assets/images/companies/img-1.png',
    name: 'New admin Design',
    description: 'It will be as simple as Occidental',
    status: 'Completed',
    color: 'success',
    dueDate: '15 Oct, 19',
    commentsCount: 214,
    team: [
      {
        id: 1,
        img: 'assets/images/users/avatar-4.jpg',
        fullname: 'Janice Cole',
        skills: [
          { id: 1, name: 'Frontend' },
          { id: 2, name: 'UI' },
        ],
      },
      {
        id: 2,
        img: 'assets/images/users/avatar-5.jpg',
        fullname: 'Steve Foster',
        skills: [{ id: 1, name: 'UI/UX' }],
      },
      {
        id: 3,
        img: 'Null',
        name: 'A',
        color: 'success',
        fullname: 'Aeffrey Walker',
        skills: [{ id: 1, name: 'Backend' }],
      },
      {
        id: 4,
        img: 'assets/images/users/avatar-2.jpg',
        fullname: 'Daniel Candles',
        skills: [
          { id: 1, name: 'Frontend' },
          { id: 2, name: 'UI' },
        ],
      },
    ],
    startDate: '08 Sept, 2019',
    projectDetails: {
      description:
        'To an English person, it will seem like simplified English, as a skeptical Cambridge friend of mine told me what Occidental is. The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc,',
      points: [
        'To achieve this, it would be necessary',
        'Separate existence is a myth.',
        'If several languages coalesce',
      ],
    },
    files: [
      { name: 'Skote Landing.Zip', size: '3.25 MB', link: '#' },
      { name: 'Skote Admin.Zip', size: '3.15 MB', link: '#' },
      { name: 'Skote Logo.Zip', size: '2.02 MB', link: '#' },
      { name: 'Veltrix admin.Zip', size: '2.25 MB', link: '#' },
    ],
    comments: [
      {
        id: 1,
        username: 'David Lambert',
        userImg: 'assets/images/users/avatar-2.jpg',
        comment: 'Separate existence is a myth.',
      },
      {
        id: 2,
        username: 'Steve Foster',
        userImg: 'assets/images/users/avatar-3.jpg',
        comment: '@Henry To an English person it will like simplified',
        reply: {
          username: 'Jeffrey Walker',
          comment: 'as a skeptical Cambridge friend',
        },
      },
      {
        id: 3,
        username: 'Steven Carlson',
        comment: ' Separate existence is a myth.',
      },
    ],
  },
  {
    id: 1,
    img: '/assets/images/companies/img-2.png',
    name: 'Brand logo design',
    description: 'To achieve it would be necessary',
    status: 'Pending',
    color: 'warning',
    dueDate: '22 Oct, 19',
    commentsCount: 183,
    team: [
      { id: 1, img: 'assets/images/users/avatar-8.jpg' },
      { id: 2, img: 'assets/images/users/avatar-2.jpg', fullname: 'Daniel Candles' },
    ],
    startDate: '08 Sept, 2019',
    projectDetails: {
      description:
        'To an English person, it will seem like simplified English, as a skeptical Cambridge friend of mine told me what Occidental is. The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc,',
      points: [
        'To achieve this, it would be necessary',
        'Separate existence is a myth.',
        'If several languages coalesce',
      ],
    },
    files: [
      { name: 'Skote Landing.Zip', size: '3.25 MB', link: '#' },
      { name: 'Skote Admin.Zip', size: '3.15 MB', link: '#' },
      { name: 'Skote Logo.Zip', size: '2.02 MB', link: '#' },
      { name: 'Veltrix admin.Zip', size: '2.25 MB', link: '#' },
    ],
    comments: [
      {
        id: 1,
        username: 'David Lambert',
        userImg: 'assets/images/users/avatar-2.jpg',
        comment: 'Separate existence is a myth.',
      },
      {
        id: 2,
        username: 'Steve Foster',
        userImg: 'assets/images/users/avatar-3.jpg',
        comment: '@Henry To an English person it will like simplified',
        reply: {
          username: 'Jeffrey Walker',
          comment: 'as a skeptical Cambridge friend',
        },
      },
      {
        id: 3,
        username: 'Steven Carlson',
        comment: ' Separate existence is a myth.',
      },
    ],
  },
  {
    id: 2,
    img: '/assets/images/companies/img-3.png',
    name: 'New Landing Design',
    description: 'For science, music, sport, etc',
    status: 'Delay',
    color: 'danger',
    dueDate: '13 Oct, 19',
    commentsCount: 175,
    team: [
      { id: 1, img: 'Null', name: 'K', color: 'info', fullname: 'Kony Brafford' },
      { id: 2, img: 'assets/images/users/avatar-2.jpg', fullname: 'Daniel Candles' },
    ],
    startDate: '08 Sept, 2019',
    projectDetails: {
      description:
        'To an English person, it will seem like simplified English, as a skeptical Cambridge friend of mine told me what Occidental is. The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc,',
      points: [
        'To achieve this, it would be necessary',
        'Separate existence is a myth.',
        'If several languages coalesce',
      ],
    },
    files: [
      { name: 'Skote Landing.Zip', size: '3.25 MB', link: '#' },
      { name: 'Skote Admin.Zip', size: '3.15 MB', link: '#' },
      { name: 'Skote Logo.Zip', size: '2.02 MB', link: '#' },
      { name: 'Veltrix admin.Zip', size: '2.25 MB', link: '#' },
    ],
    comments: [
      {
        id: 1,
        username: 'David Lambert',
        userImg: 'assets/images/users/avatar-2.jpg',
        comment: 'Separate existence is a myth.',
      },
      {
        id: 2,
        username: 'Steve Foster',
        userImg: 'assets/images/users/avatar-3.jpg',
        comment: '@Henry To an English person it will like simplified',
        reply: {
          username: 'Jeffrey Walker',
          comment: 'as a skeptical Cambridge friend',
        },
      },
      {
        id: 3,
        username: 'Steven Carlson',
        comment: ' Separate existence is a myth.',
      },
    ],
  },
  {
    id: 3,
    img: '/assets/images/companies/img-4.png',
    name: 'Redesign - Landing page',
    description: 'If several languages coalesce',
    status: 'Completed',
    color: 'success',
    dueDate: '14 Oct, 19',
    commentsCount: 202,
    team: [{ id: 1, img: 'assets/images/users/avatar-4.jpg', fullname: 'Janice Cole' }],
    startDate: '08 Sept, 2019',
    projectDetails: {
      description:
        'To an English person, it will seem like simplified English, as a skeptical Cambridge friend of mine told me what Occidental is. The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc,',
      points: [
        'To achieve this, it would be necessary',
        'Separate existence is a myth.',
        'If several languages coalesce',
      ],
    },
    files: [
      { name: 'Skote Landing.Zip', size: '3.25 MB', link: '#' },
      { name: 'Skote Admin.Zip', size: '3.15 MB', link: '#' },
      { name: 'Skote Logo.Zip', size: '2.02 MB', link: '#' },
      { name: 'Veltrix admin.Zip', size: '2.25 MB', link: '#' },
    ],
    comments: [
      {
        id: 1,
        username: 'David Lambert',
        userImg: 'assets/images/users/avatar-2.jpg',
        comment: 'Separate existence is a myth.',
      },
      {
        id: 2,
        username: 'Steve Foster',
        userImg: 'assets/images/users/avatar-3.jpg',
        comment: '@Henry To an English person it will like simplified',
        reply: {
          username: 'Jeffrey Walker',
          comment: 'as a skeptical Cambridge friend',
        },
      },
      {
        id: 3,
        username: 'Steven Carlson',
        comment: ' Separate existence is a myth.',
      },
    ],
  },
  {
    id: 4,
    img: '/assets/images/companies/img-5.png',
    name: 'Skote Dashboard UI',
    description: 'Separate existence is a myth',
    status: 'Completed',
    color: 'success',
    dueDate: '13 Oct, 19',
    commentsCount: 194,
    team: [
      { id: 1, img: 'assets/images/users/avatar-1.jpg', fullname: 'Jennifer Walker' },
      { id: 2, img: 'assets/images/users/avatar-3.jpg', fullname: 'Daniel Candel' },
      { id: 3, img: 'Null', name: '3+', color: 'danger' },
    ],
    startDate: '08 Sept, 2019',
    projectDetails: {
      description:
        'To an English person, it will seem like simplified English, as a skeptical Cambridge friend of mine told me what Occidental is. The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc,',
      points: [
        'To achieve this, it would be necessary',
        'Separate existence is a myth.',
        'If several languages coalesce',
      ],
    },
    files: [
      { name: 'Skote Landing.Zip', size: '3.25 MB', link: '#' },
      { name: 'Skote Admin.Zip', size: '3.15 MB', link: '#' },
      { name: 'Skote Logo.Zip', size: '2.02 MB', link: '#' },
      { name: 'Veltrix admin.Zip', size: '2.25 MB', link: '#' },
    ],
    comments: [
      {
        id: 1,
        username: 'David Lambert',
        userImg: 'assets/images/users/avatar-2.jpg',
        comment: 'Separate existence is a myth.',
      },
      {
        id: 2,
        username: 'Steve Foster',
        userImg: 'assets/images/users/avatar-3.jpg',
        comment: '@Henry To an English person it will like simplified',
        reply: {
          username: 'Jeffrey Walker',
          comment: 'as a skeptical Cambridge friend',
        },
      },
      {
        id: 3,
        username: 'Steven Carlson',
        comment: ' Separate existence is a myth.',
      },
    ],
  },
  {
    id: 5,
    img: '/assets/images/companies/img-6.png',
    name: 'Blog Template UI',
    description: 'For science, music, sport, etc',
    status: 'Pending',
    color: 'warning',
    dueDate: '24 Oct, 19',
    commentsCount: 222,
    team: [
      { id: 1, img: 'assets/images/users/avatar-4.jpg', fullname: 'Janice Cole' },
      { id: 2, img: 'assets/images/users/avatar-5.jpg', fullname: 'Steve Foster' },
    ],
    startDate: '08 Sept, 2019',
    projectDetails: {
      description:
        'To an English person, it will seem like simplified English, as a skeptical Cambridge friend of mine told me what Occidental is. The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc,',
      points: [
        'To achieve this, it would be necessary',
        'Separate existence is a myth.',
        'If several languages coalesce',
      ],
    },
    files: [
      { name: 'Skote Landing.Zip', size: '3.25 MB', link: '#' },
      { name: 'Skote Admin.Zip', size: '3.15 MB', link: '#' },
      { name: 'Skote Logo.Zip', size: '2.02 MB', link: '#' },
      { name: 'Veltrix admin.Zip', size: '2.25 MB', link: '#' },
    ],
    comments: [
      {
        id: 1,
        username: 'David Lambert',
        userImg: 'assets/images/users/avatar-2.jpg',
        comment: 'Separate existence is a myth.',
      },
      {
        id: 2,
        username: 'Steve Foster',
        userImg: 'assets/images/users/avatar-3.jpg',
        comment: '@Henry To an English person it will like simplified',
        reply: {
          username: 'Jeffrey Walker',
          comment: 'as a skeptical Cambridge friend',
        },
      },
      {
        id: 3,
        username: 'Steven Carlson',
        comment: ' Separate existence is a myth.',
      },
    ],
  },
  {
    id: 6,
    img: '/assets/images/companies/img-3.png',
    name: 'Multipurpose Landing',
    description: 'It will be as simple as Occidental',
    status: 'Delay',
    color: 'danger',
    dueDate: '15 Oct, 19',
    commentsCount: 214,
    team: [
      { id: 1, img: 'assets/images/users/avatar-5.jpg', fullname: 'Steve Foster' },
      { id: 3, img: 'Null', name: 'R', color: 'warning', fullname: 'Rony Candles' },
    ],
  },
  {
    id: 7,
    img: '/assets/images/companies/img-4.png',
    name: 'App Landing UI',
    description: 'It will be as simple as Occidental',
    status: 'Completed',
    color: 'success',
    dueDate: '11 Oct, 19',
    commentsCount: 185,
    team: [
      { id: 1, img: 'Null', name: 'L', color: 'pink', fullname: 'Lony Mackay' },
      { id: 3, img: 'assets/images/users/avatar-2.jpg', fullname: 'Daniel Candles' },
    ],
    startDate: '08 Sept, 2019',
    projectDetails: {
      description:
        'To an English person, it will seem like simplified English, as a skeptical Cambridge friend of mine told me what Occidental is. The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc,',
      points: [
        'To achieve this, it would be necessary',
        'Separate existence is a myth.',
        'If several languages coalesce',
      ],
    },
    files: [
      { name: 'Skote Landing.Zip', size: '3.25 MB', link: '#' },
      { name: 'Skote Admin.Zip', size: '3.15 MB', link: '#' },
      { name: 'Skote Logo.Zip', size: '2.02 MB', link: '#' },
      { name: 'Veltrix admin.Zip', size: '2.25 MB', link: '#' },
    ],
    comments: [
      {
        id: 1,
        username: 'David Lambert',
        userImg: 'assets/images/users/avatar-2.jpg',
        comment: 'Separate existence is a myth.',
      },
      {
        id: 2,
        username: 'Steve Foster',
        userImg: 'assets/images/users/avatar-3.jpg',
        comment: '@Henry To an English person it will like simplified',
        reply: {
          username: 'Jeffrey Walker',
          comment: 'as a skeptical Cambridge friend',
        },
      },
      {
        id: 3,
        username: 'Steven Carlson',
        comment: ' Separate existence is a myth.',
      },
    ],
  },
  {
    id: 8,
    img: '/assets/images/companies/img-2.png',
    name: 'New admin Design',
    description: 'Their most common words.',
    status: 'Completed',
    color: 'success',
    dueDate: '12 Oct, 19',
    commentsCount: 106,
    team: [
      { id: 1, img: 'assets/images/users/avatar-4.jpg', fullname: 'Janice Cole' },
      { id: 2, img: 'assets/images/users/avatar-5.jpg', fullname: 'Steve Foster' },
      { id: 3, img: 'Null', name: 'A', color: 'success', fullname: 'Aeffrey Walker' },
      { id: 3, img: 'assets/images/users/avatar-2.jpg', fullname: 'Daniel Candles' },
    ],
    startDate: '08 Sept, 2019',
    projectDetails: {
      description:
        'To an English person, it will seem like simplified English, as a skeptical Cambridge friend of mine told me what Occidental is. The European languages are members of the same family. Their separate existence is a myth. For science, music, sport, etc,',
      points: [
        'To achieve this, it would be necessary',
        'Separate existence is a myth.',
        'If several languages coalesce',
      ],
    },
    files: [
      { name: 'Skote Landing.Zip', size: '3.25 MB', link: '#' },
      { name: 'Skote Admin.Zip', size: '3.15 MB', link: '#' },
      { name: 'Skote Logo.Zip', size: '2.02 MB', link: '#' },
      { name: 'Veltrix admin.Zip', size: '2.25 MB', link: '#' },
    ],
    comments: [
      {
        id: 1,
        username: 'David Lambert',
        userImg: 'assets/images/users/avatar-2.jpg',
        comment: 'Separate existence is a myth.',
      },
      {
        id: 2,
        username: 'Steve Foster',
        userImg: 'assets/images/users/avatar-3.jpg',
        comment: '@Henry To an English person it will like simplified',
        reply: {
          username: 'Jeffrey Walker',
          comment: 'as a skeptical Cambridge friend',
        },
      },
      {
        id: 3,
        username: 'Steven Carlson',
        comment: ' Separate existence is a myth.',
      },
    ],
  },
]

const options = {
  chart: {
    height: 290,
    type: 'bar',
    toolbar: {
      show: !1,
    },
  },
  plotOptions: {
    bar: {
      columnWidth: '14%',
      endingShape: 'rounded',
    },
  },
  dataLabels: {
    enabled: !1,
  },
  series: [
    {
      name: 'Overview',
      data: [42, 56, 40, 64, 26, 42, 56, 35, 62],
    },
  ],
  grid: {
    yaxis: {
      lines: {
        show: !1,
      },
    },
  },
  yaxis: {
    title: {
      text: '% (Percentage)',
    },
  },
  xaxis: {
    labels: {
      rotate: -90,
    },
    categories: ['1', '2', '3', '4', '5', '6', '7', '8', '9'],
    title: {
      text: 'Week',
    },
  },
  colors: ['#556ee6'],
}

const series = [
  {
    name: 'Overview',
    data: [42, 56, 40, 64, 26, 42, 56, 35, 62],
  },
]

const MOCK_DATA = {
  projects,
  options,
  series,
}

export default MOCK_DATA
