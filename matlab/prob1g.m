function [x,y]=prob1g(bs,s)
%PROB1G	Gives geometry data for the prob1g PDE model.
%
%   NE=PROB1G gives the number of boundary segments
%
%   D=PROB1G(BS) gives a matrix with one column for each boundary segment
%   specified in BS.
%   Row 1 contains the start parameter value.
%   Row 2 contains the end parameter value.
%   Row 3 contains the number of the left-hand regions.
%   Row 4 contains the number of the right-hand regions.
%
%   [X,Y]=PROB1G(BS,S) gives coordinates of boundary points. BS specifies the
%   boundary segments and S the corresponding parameter values. BS may be
%   a scalar.

nbs=8;

if nargin==0,
  x=nbs; % number of boundary segments
  return
end

d=[
  0 0 0 0 0 0 0 0 % start parameter value
  1 1 1 1 1 1 1 1 % end parameter value
  0 0 1 1 1 0 0 1 % left hand region
  1 1 0 0 0 1 1 0 % right hand region
];

bs1=bs(:)';

if find(bs1<1 | bs1>nbs),
  error(message('pde:wgeom:NonExistBoundSeg'))
end

if nargin==1,
  x=d(:,bs1);
  return
end

x=zeros(size(s));
y=zeros(size(s));
[m,n]=size(bs);
if m==1 & n==1,
  bs=bs*ones(size(s)); % expand bs
elseif m~=size(s,1) | n~=size(s,2),
  error(message('pde:wgeom:BsSizeError'));
end

if ~isempty(s),

% boundary segment 1
ii=find(bs==1);
if length(ii)
x(ii)=(6.3894422310756953-(6.3894422310756953))*(s(ii)-d(1,1))/(d(2,1)-d(1,1))+(6.3894422310756953);
y(ii)=(2.0418326693227087-(6.0607569721115562))*(s(ii)-d(1,1))/(d(2,1)-d(1,1))+(6.0607569721115562);
end

% boundary segment 2
ii=find(bs==2);
if length(ii)
x(ii)=(2.8237051792828689-(6.3894422310756953))*(s(ii)-d(1,2))/(d(2,2)-d(1,2))+(6.3894422310756953);
y(ii)=(2.0418326693227087-(2.0418326693227087))*(s(ii)-d(1,2))/(d(2,2)-d(1,2))+(2.0418326693227087);
end

% boundary segment 3
ii=find(bs==3);
if length(ii)
x(ii)=(5.0249003984063743-(5.0249003984063743))*(s(ii)-d(1,3))/(d(2,3)-d(1,3))+(5.0249003984063743);
y(ii)=(3.5806772908366558-(4.8057768924302815))*(s(ii)-d(1,3))/(d(2,3)-d(1,3))+(4.8057768924302815);
end

% boundary segment 4
ii=find(bs==4);
if length(ii)
x(ii)=(4.0687250996015933-(5.0249003984063743))*(s(ii)-d(1,4))/(d(2,4)-d(1,4))+(5.0249003984063743);
y(ii)=(3.5806772908366558-(3.5806772908366558))*(s(ii)-d(1,4))/(d(2,4)-d(1,4))+(3.5806772908366558);
end

% boundary segment 5
ii=find(bs==5);
if length(ii)
x(ii)=(5.0249003984063743-(4.0687250996015933))*(s(ii)-d(1,5))/(d(2,5)-d(1,5))+(4.0687250996015933);
y(ii)=(4.8057768924302815-(4.8057768924302815))*(s(ii)-d(1,5))/(d(2,5)-d(1,5))+(4.8057768924302815);
end

% boundary segment 6
ii=find(bs==6);
if length(ii)
x(ii)=(2.8237051792828689-(2.8237051792828689))*(s(ii)-d(1,6))/(d(2,6)-d(1,6))+(2.8237051792828689);
y(ii)=(6.0607569721115562-(2.0418326693227087))*(s(ii)-d(1,6))/(d(2,6)-d(1,6))+(2.0418326693227087);
end

% boundary segment 7
ii=find(bs==7);
if length(ii)
x(ii)=(6.3894422310756953-(2.8237051792828689))*(s(ii)-d(1,7))/(d(2,7)-d(1,7))+(2.8237051792828689);
y(ii)=(6.0607569721115562-(6.0607569721115562))*(s(ii)-d(1,7))/(d(2,7)-d(1,7))+(6.0607569721115562);
end

% boundary segment 8
ii=find(bs==8);
if length(ii)
x(ii)=(4.0687250996015933-(4.0687250996015933))*(s(ii)-d(1,8))/(d(2,8)-d(1,8))+(4.0687250996015933);
y(ii)=(4.8057768924302815-(3.5806772908366558))*(s(ii)-d(1,8))/(d(2,8)-d(1,8))+(3.5806772908366558);
end

end
