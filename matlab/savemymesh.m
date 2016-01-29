dlmwrite('p.out',['##Point'],'-append','delimiter','');
dlmwrite('p.out', p,'-append','delimiter',' ');
dlmwrite('p.out',['##Triangle'],'-append','delimiter','');
dlmwrite('p.out',t,'-append','delimiter',' ');
dlmwrite('p.out',['##Boundary'],'-append','delimiter','');
dlmwrite('p.out',e,'-append','delimiter',' ');



